﻿using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SampSharp.Entities;

/// <summary>Provides a compiler for an invoke method for an instance method with injected dependencies and entity-to-component conversion.</summary>
internal static class MethodInvokerFactory
{
    private static readonly MethodInfo _getComponentInfo = typeof(IEntityManager).GetMethod(nameof(IEntityManager.GetComponent),
        BindingFlags.Public | BindingFlags.Instance, null, [typeof(EntityId)], null)!;

    private static readonly MethodInfo _getServiceInfo =
        typeof(MethodInvokerFactory).GetMethod(nameof(GetService), BindingFlags.NonPublic | BindingFlags.Static)!;

    /// <summary>Compiles the invoker for the specified method.</summary>
    /// <param name="methodInfo">The method information.</param>
    /// <param name="parameterSources">The sources of the parameters.</param>
    /// <param name="uninvokedReturnValue">The value returned if the method is not invoked when a parameter could not be converted to the correct component.</param>
    /// <param name="retBoolToResult">Indicates whether the <see langword="bool" /> return value should be converted to a <see cref="MethodResult" /> value.</param>
    /// <returns>The method invoker.</returns>
    public static MethodInvoker Compile(MethodInfo methodInfo, MethodParameterSource[] parameterSources, object? uninvokedReturnValue = null, bool retBoolToResult = true)
    {
        if (methodInfo.DeclaringType == null)
        {
            throw new ArgumentException("Method must have declaring type", nameof(methodInfo));
        }

        // Input arguments
        var instanceArg = Expression.Parameter(typeof(object), "instance");
        var argsArg = Expression.Parameter(typeof(object[]), "args");
        var serviceProviderArg = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");
        var entityManagerArg = Expression.Parameter(typeof(IEntityManager), "entityManager");
        var entityEmpty = Expression.Constant(EntityId.Empty, typeof(EntityId));
        Expression? argsCheckExpression = null;

        var locals = new List<ParameterExpression>();
        var expressions = new List<Expression>();
        var methodArguments = new Expression[parameterSources.Length];

        for (var i = 0; i < parameterSources.Length; i++)
        {
            var source = parameterSources[i];
            var parameterType = source
                .Info.ParameterType;
            if (parameterType.IsByRef)
            {
                throw new NotSupportedException("Reference parameters are not supported");
            }

            if (source.IsComponent)
            {
                // Get component from entity

                // Declare local variables
                var entityArg = Expression.Parameter(typeof(EntityId), $"entity{i}");
                var componentArg = Expression.Parameter(source
                        .Info.ParameterType, $"component{i}");
                var componentNull = Expression.Constant(null, source.Info.ParameterType);

                locals.Add(entityArg);
                locals.Add(componentArg);

                // Constant index in args array
                Expression index = Expression.Constant(source.ParameterIndex);

                // Assign entity from args array to entity variable.
                var getEntityExpression = Expression.Assign(entityArg, Expression.Convert(Expression.ArrayIndex(argsArg, index), typeof(EntityId)));
                expressions.Add(getEntityExpression);

                // If entity is not null, convert entity to component. Assign component to component variable.
                var getComponentInfo = _getComponentInfo.MakeGenericMethod(source.Info.ParameterType);
                var getComponentExpression = Expression.Assign(componentArg,
                    Expression.Condition(Expression.Equal(entityArg, entityEmpty), componentNull,
                        Expression.Call(entityManagerArg, getComponentInfo, entityArg)));
                expressions.Add(getComponentExpression);

                // If an entity was provided in the args list, the entity must be convertible to the component. Add
                // check for entity to either be null or the component to not be null.
                var checkExpression = Expression.OrElse(Expression.Equal(entityArg, entityEmpty), Expression.NotEqual(componentArg, componentNull));

                argsCheckExpression = argsCheckExpression == null
                    ? checkExpression
                    : Expression.AndAlso(argsCheckExpression, checkExpression);

                // Add component variable as the method argument.
                methodArguments[i] = componentArg;
            }
            else if (source.IsService)
            {
                // Get service
                var getServiceCall = Expression.Call(_getServiceInfo, serviceProviderArg, Expression.Constant(parameterType, typeof(Type)));
                methodArguments[i] = Expression.Convert(getServiceCall, parameterType);
            }
            else if (source.ParameterIndex >= 0)
            {
                // Pass through
                Expression index = Expression.Constant(source.ParameterIndex);

                var getValue = Expression.ArrayIndex(argsArg, index);
                methodArguments[i] = Expression.Convert(getValue, parameterType);
            }
        }

        var service = Expression.Convert(instanceArg, methodInfo.DeclaringType);
        Expression body = Expression.Call(service, methodInfo, methodArguments);

        if (body.Type == typeof(void))
        {
            body = Expression.Block(body, Expression.Constant(null));
        }
        else if (retBoolToResult && body.Type == typeof(bool))
        {
            var boxMethod = typeof(MethodResult).GetMethod(nameof(MethodResult.From))!;
            body = Expression.Call(boxMethod, body);
        }
        else if (body.Type != typeof(object))
        {
            body = Expression.Convert(body, typeof(object));
        }

        if (argsCheckExpression != null)
        {
            body = Expression.Condition(argsCheckExpression, body, Expression.Constant(uninvokedReturnValue, typeof(object)));
        }

        if (locals.Count > 0 || expressions.Count > 0)
        {
            expressions.Add(body);
            body = Expression.Block(locals, expressions);
        }

        var lambda = Expression.Lambda<MethodInvoker>(body, instanceArg, argsArg, serviceProviderArg, entityManagerArg);

        return lambda.Compile();
    }

    private static object GetService(IServiceProvider serviceProvider, Type type)
    {
        return serviceProvider.GetRequiredService(type);
    }
}
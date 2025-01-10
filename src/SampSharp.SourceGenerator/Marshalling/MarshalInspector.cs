using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Helpers;
using SampSharp.SourceGenerator.Marshalling.Shapes;

namespace SampSharp.SourceGenerator.Marshalling;

public static class MarshalInspector
{
    public static MarshalMembers GetMembers(CustomMarshallerInfo info)
    {
        return new MarshalMembers(
            StatefulFreeMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodFree), 
            StatefulFromManagedMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodFromManaged, false, info.ManagedType),
            StatefulFromManagedWithBufferMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodFromManaged, false, x => x.Type.IsSame(info.ManagedType), x => IsSpanByte(x.Type)),
            StatefulFromUnmanagedMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodFromUnmanaged, parameterCount: 1),

            StatefulToManagedMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodToManaged),
            StatefulToManagedFinallyMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodToManagedFinally),
            StatefulToUnmanagedMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodToUnmanaged),

            StatefulOnInvokedMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodOnInvoked),
            StatefulGetPinnableReferenceMethod: GetMethod(info.MarshallerType, true, ShapeConstants.MethodGetPinnableReference, true),
            StatelessGetPinnableReferenceMethod: GetMethod(info.MarshallerType, false, ShapeConstants.MethodGetPinnableReference, true, info.ManagedType),

            StatelessConvertToUnmanagedMethod: GetMethod(info.MarshallerType, false, ShapeConstants.MethodConvertToUnmanaged, false, info.ManagedType),
            StatelessConvertToUnmanagedWithBufferMethod: GetMethod(info.MarshallerType, false, ShapeConstants.MethodConvertToUnmanaged, false, x => x.Type.IsSame(info.ManagedType), x => IsSpanByte(x.Type)),
            StatelessConvertToManagedMethod: GetMethod(info.MarshallerType, false, ShapeConstants.MethodConvertToManaged, parameterCount: 1),
            StatelessConvertToManagedFinallyMethod: GetMethod(info.MarshallerType, false, ShapeConstants.MethodConvertToManagedFinally, parameterCount: 1),
            StatelessFreeMethod: GetMethod(info.MarshallerType, false, ShapeConstants.MethodFree, parameterCount: 1),

            BufferSizeProperty: GetStaticProperty(info.MarshallerType, ShapeConstants.PropertyBufferSize, x => x.SpecialType == SpecialType.System_Int32)
        );
    }

    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name, bool returnsByRef = false, int parameterCount = 0)
    {
        return type
            .GetMembers(name)
            .OfType<IMethodSymbol>()
            .FirstOrDefault(x =>
            {
                if (stateful == x.IsStatic || x.Parameters.Length != parameterCount || x.ReturnsByRef != returnsByRef)
                {
                    return false;
                }

                return !Array.Empty<ITypeSymbol>().Where((t, i) => !SymbolExtensions.IsSame((ISymbol)x.Parameters[i].Type, t))
                    .Any();
            });
    }

    
    private static bool IsSpanByte(ITypeSymbol type)
    {
        return type is INamedTypeSymbol named && named.ToDisplayString() == Constants.SpanOfBytesFQN;
    }

    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name, bool returnsByRef = false, params ITypeSymbol[] paramTypes)
    {
        return type
            .GetMembers(name)
            .OfType<IMethodSymbol>()
            .FirstOrDefault(x =>
            {
                if (stateful == x.IsStatic || x.Parameters.Length != paramTypes.Length)
                {
                    return false;
                }

                if (x.ReturnsByRef != returnsByRef)
                {
                    return false;
                }

                return !paramTypes.Where((t, i) => !x.Parameters[i].Type.IsSame(t))
                    .Any();
            });

    }
    
    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name, bool returnsByRef, params Func<IParameterSymbol, bool>[] paramTypes)
    {
        return type
            .GetMembers(name)
            .OfType<IMethodSymbol>()
            .FirstOrDefault(x =>
            {
                if (stateful == x.IsStatic || x.Parameters.Length != paramTypes.Length)
                {
                    return false;
                }

                if (x.ReturnsByRef != returnsByRef)
                {
                    return false;
                }

                return !paramTypes.Where((check, i) => !check(x.Parameters[i]))
                    .Any();
            });

    }

    private static IPropertySymbol? GetStaticProperty(ITypeSymbol type, string name, Func<ITypeSymbol, bool> propertyType)
    {
        return type
            .GetMembers(name)
            .OfType<IPropertySymbol>()
            .FirstOrDefault(x => x.IsStatic && propertyType(x.Type));
    }
}
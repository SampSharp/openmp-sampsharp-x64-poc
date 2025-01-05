using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Helpers;
using SampSharp.SourceGenerator.Marshalling.Shapes;

namespace SampSharp.SourceGenerator.Marshalling;

public class MarshallerShapeFactory
{
    private readonly WellKnownMarshallerTypes _wellKnownMarshallerTypes;

    public MarshallerShapeFactory(WellKnownMarshallerTypes wellKnownMarshallerTypes)
    {
        _wellKnownMarshallerTypes = wellKnownMarshallerTypes;
    }

    /// <summary>
    /// Returns the marshaller shape for the return value of the specified <paramref name="method"/>.
    /// </summary>
    public IMarshallerShape? GetMarshallerShape(IMethodSymbol method, MarshallingDirection direction)
    {
        if (method.ReturnsVoid)
        {
            return null;
        }

        var marshalUsing = method.GetReturnTypeAttribute(Constants.MarshalUsingAttributeFQN);
        var typeMarshaller = method.ReturnType.GetAttribute(Constants.NativeMarshallingAttributeFQN);
        
        var marshallerType = GetMarshallerTypeForParameterType(typeMarshaller, marshalUsing, method.ReturnType);

        // Always handle return parameter as "out"
        return GetMarshallerShape(marshallerType, method.ReturnType, RefKind.Out, GetDirectionInfo(direction));
    }

    /// <summary>
    /// Returns the marshaller shape for the specified <paramref name="parameter"/>.
    /// </summary>
    public IMarshallerShape? GetMarshallerShape(IParameterSymbol parameter, MarshallingDirection direction)
    {
        var marshalUsing = parameter.GetAttribute(Constants.MarshalUsingAttributeFQN);
        var typeMarshaller = parameter.Type.GetAttribute(Constants.NativeMarshallingAttributeFQN);
        
        var marshallerType = GetMarshallerTypeForParameterType(typeMarshaller, marshalUsing, parameter.Type);

        return GetMarshallerShape(marshallerType, parameter.Type, parameter.RefKind, GetDirectionInfo(direction));
    }

    private static MarshallingDirectionInfo GetDirectionInfo(MarshallingDirection direction)
    {
        return direction switch
        {
            MarshallingDirection.ManagedToUnmanaged => MarshallingDirectionInfo.ManagedToUnmanaged,
            MarshallingDirection.UnmanagedToManaged => MarshallingDirectionInfo.UnmanagedToManaged,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    private static IMarshallerShape? GetMarshallerShape(ITypeSymbol? marshallerType, ITypeSymbol parameterType, RefKind refKind, MarshallingDirectionInfo direction)
    {
        if (marshallerType == null)
        {
            return null;
        }

        var filteredModes = GetModes(marshallerType, parameterType)
            .Where(x => parameterType.IsSame(x.ManagedType))
            .ToList();

        if (filteredModes.Count == 0)
        {
            return null;
        }

        var marshallerMode = refKind 
            switch
        {
            RefKind.In or RefKind.RefReadOnlyParameter or RefKind.None => filteredModes.FirstOrDefault(x => x.Mode == direction.In),
            RefKind.Out => filteredModes.FirstOrDefault(x => x.Mode == direction.Out),
            RefKind.Ref => filteredModes.FirstOrDefault(x => x.Mode == direction.Ref),
            _ => null
        };

        var defaultInfo = filteredModes.FirstOrDefault(x => x.Mode == MarshallerModeValue.Default);

        var shape = marshallerMode == null 
            ? null 
            : GetMarshallerShapeForMarshallerMode(marshallerMode, refKind);

        if (shape == null && defaultInfo != null)
        {
            shape = GetMarshallerShapeForMarshallerMode(defaultInfo, refKind);
        }

        return shape;
    }

    private ITypeSymbol? GetMarshallerTypeForParameterType( AttributeData? typeMarshallerAttrib, AttributeData? marshalUsingAttrib,
        ITypeSymbol type)
    {
        var marshaller = typeMarshallerAttrib?.ConstructorArguments[0].Value as ITypeSymbol;
        if (marshalUsingAttrib?.ConstructorArguments.Length > 0)
        {
            if (marshalUsingAttrib.ConstructorArguments[0].Value is ITypeSymbol marshallerOverride)
            {
                marshaller = marshallerOverride;
            }
        }

        // If no marshaller specified, look at a matching well known marshaller
        if (marshaller != null)
        {
            return marshaller;
        }

        return _wellKnownMarshallerTypes.Marshallers
            .FirstOrDefault(x => x.matcher(type) && x.marshaller != null)
            .marshaller;
    }

    private static IMarshallerShape? GetMarshallerShapeForMarshallerMode(MarshallerModeInfo marshallerInfo, RefKind refKind)
    {
        if (marshallerInfo.MarshallerType.IsStatic)
        {
            // stateless
            return refKind switch
            {
                RefKind.In or RefKind.RefReadOnlyParameter or RefKind.None => MarshallerShapeActivator.GetStatelessManagedToUnmanaged(marshallerInfo, refKind),
                RefKind.Out => MarshallerShapeActivator.GetStatelessUnmanagedToManaged(marshallerInfo),
                RefKind.Ref => MarshallerShapeActivator.GetStatelessBidirectional(marshallerInfo, refKind),
                _ => null
            };
        }

        if (marshallerInfo.MarshallerType.IsValueType)
        {
            // stateful
            return refKind switch
            {
                RefKind.In or RefKind.RefReadOnlyParameter or RefKind.None => MarshallerShapeActivator.GetStatefulManagedToUnmanaged(marshallerInfo, refKind),
                RefKind.Out => MarshallerShapeActivator.GetStatefulUnmanagedToManaged(marshallerInfo),
                RefKind.Ref => MarshallerShapeActivator.GetStatefulBidirectional(marshallerInfo, refKind),
                _ => null
            };
        }

        return null;
    }

    /// <summary>
    /// Returns all available marshaller modes for the specified marshaller type.
    /// </summary>
    private static MarshallerModeInfo[] GetModes(ITypeSymbol marshaller, ITypeSymbol forType)
    {
        return marshaller.GetAttributes(Constants.CustomMarshallerAttributeFQN)
            .Select(x => GetModeFromAttribute(x, forType))
            .WhereNotNull()
            .ToArray();
    }

    /// <summary>
    /// Returns marshaller mode info for the specified [CustomMarshaller] attribute.
    /// </summary>
    private static MarshallerModeInfo? GetModeFromAttribute(AttributeData attributeData, ITypeSymbol forType)
    {
        var managedType = (ITypeSymbol)attributeData.ConstructorArguments[0].Value!;
        var mode = ModeForValue(attributeData.ConstructorArguments[1].Value!);

        if (attributeData.ConstructorArguments[2].Value! is not INamedTypeSymbol marshallerType)
        {
            return null;
        }

        if (managedType.IsSame(Constants.GenericPlaceholderFQN))
        {
            managedType = forType;
        }

        // Replace generic placeholders with the actual type
        if (marshallerType is { IsGenericType: true })
        {
            marshallerType = ReplacePlaceholderWithType(marshallerType, forType);
        }

        if (marshallerType.ContainingType is { IsGenericType: true })
        {
            var containing = ReplacePlaceholderWithType(marshallerType.ContainingType, forType);

            // TODO: might not work properly for nested generic types
            marshallerType = containing.GetMembers(marshallerType.Name)
                .OfType<INamedTypeSymbol>()
                .First();
        }

        
        return new MarshallerModeInfo(managedType, mode, marshallerType);
    }

    private static INamedTypeSymbol ReplacePlaceholderWithType(INamedTypeSymbol namedType, ITypeSymbol type)
    {
        var replacements = 0;
        var typeArguments = namedType.TypeArguments;
        while (typeArguments.Any(x => x.IsSame(Constants.GenericPlaceholderFQN)))
        {
            var placeholder = namedType.TypeArguments.First(x => x.IsSame(Constants.GenericPlaceholderFQN));
    
            typeArguments = typeArguments.Replace(placeholder, type);
            replacements++;
        }
    
        return replacements > 0 
            ? namedType.ConstructedFrom.Construct(typeArguments.ToArray()) 
            : namedType;
    }

    private static MarshallerModeValue ModeForValue(object constant)
    {
        return constant is int number 
            ? (MarshallerModeValue)number 
            : MarshallerModeValue.Other;
    }
}
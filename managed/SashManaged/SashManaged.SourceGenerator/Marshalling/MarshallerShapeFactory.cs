using System.Linq;
using Microsoft.CodeAnalysis;
using SashManaged.SourceGenerator.Marshalling.Shapes;

namespace SashManaged.SourceGenerator.Marshalling;

public static class MarshallerShapeFactory
{
    /// <summary>
    /// Returns the marshaller shape for the return value of the specified method <paramref name="symbol"/>.
    /// </summary>
    public static IMarshallerShape? GetMarshallerShape(IMethodSymbol symbol, WellKnownMarshallerTypes wellKnownMarshallerTypes)
    {
        if (symbol.ReturnsVoid)
        {
            return null;
        }

        var marshalUsing = symbol.GetReturnTypeAttribute(Constants.MarshalUsingAttributeFQN);
        var typeMarshaller = symbol.ReturnType.GetAttribute(Constants.NativeMarshallingAttributeFQN);
        
        // Always handle return parameter as "out"
        return GetMarshallerShape(wellKnownMarshallerTypes, typeMarshaller, marshalUsing, symbol.ReturnType, RefKind.Out);
    }

    /// <summary>
    /// Returns the marshaller shape for the specified parameter <paramref name="symbol"/>.
    /// </summary>
    public static IMarshallerShape? GetMarshallerShape(IParameterSymbol symbol, WellKnownMarshallerTypes wellKnownMarshallerTypes)
    {
        var refKind = symbol.RefKind;
        var type = symbol.Type;

        var marshalUsing = symbol.GetAttribute(Constants.MarshalUsingAttributeFQN);
        var typeMarshaller = symbol.Type.GetAttribute(Constants.NativeMarshallingAttributeFQN);
        
        return GetMarshallerShape(wellKnownMarshallerTypes, typeMarshaller, marshalUsing, type, refKind);
    }

    private static IMarshallerShape? GetMarshallerShape(WellKnownMarshallerTypes wellKnownMarshallerTypes, AttributeData? typeMarshaller, AttributeData? marshalUsing,
        ITypeSymbol type, RefKind refKind)
    {
        var marshaller = typeMarshaller?.ConstructorArguments[0].Value as ITypeSymbol;
        if (marshalUsing?.ConstructorArguments.Length > 0)
        {
            if (marshalUsing.ConstructorArguments[0].Value is ITypeSymbol marshallerOverride)
            {
                marshaller = marshallerOverride;
            }
        }

        // If no marshaller specified, look at a matching well known marshaller
        if (marshaller == null)
        {
            var wellKnownMarshaller = wellKnownMarshallerTypes.Marshallers
                .FirstOrDefault(x => x.matcher(type) && x.marshaller != null)
                .marshaller;

            if (wellKnownMarshaller == null)
            {
                return null;
            }

            marshaller = wellKnownMarshaller;
        }

        var filteredModes = GetModes(marshaller, type)
            .Where(x => type.IsSame(x.ManagedType))
            .ToList();

        if (filteredModes.Count == 0)
        {
            return null;
        }

        var marshallerMode = refKind 
            switch
        {
            RefKind.In or RefKind.RefReadOnlyParameter or RefKind.None => filteredModes.FirstOrDefault(x => x.Mode == MarshallerModeValue.ManagedToUnmanagedIn),
            RefKind.Out => filteredModes.FirstOrDefault(x => x.Mode == MarshallerModeValue.ManagedToUnmanagedOut),
            RefKind.Ref => filteredModes.FirstOrDefault(x => x.Mode == MarshallerModeValue.ManagedToUnmanagedRef),
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
            .Where(x => x != null)
            .Select(x => x!)
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
    
        if (replacements > 0)
        {
            return namedType.ConstructedFrom.Construct(typeArguments.ToArray());
        }
    
        return namedType;
    }

    private static MarshallerModeValue ModeForValue(object constant)
    {
        return constant is int number 
            ? (MarshallerModeValue)number 
            : MarshallerModeValue.Other;
    }

}
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

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

        var filteredModes = GetModes(marshaller)
            .Where(x => SymbolEqualityComparer.Default.Equals(type, x.ManagedType))
            .ToList();

        if (filteredModes.Count == 0)
        {
            return null;
        }

        var marshallerMode = refKind 
            switch
        {
            RefKind.In or RefKind.None => filteredModes.FirstOrDefault(x => x.Mode == MarshallerModeValue.ManagedToUnmanagedIn),
            RefKind.Out => filteredModes.FirstOrDefault(x => x.Mode == MarshallerModeValue.ManagedToUnmanagedOut),
            RefKind.Ref or RefKind.RefReadOnlyParameter => filteredModes.FirstOrDefault(x => x.Mode == MarshallerModeValue.ManagedToUnmanagedRef),
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
                RefKind.In or RefKind.None => MarshallerShapeActivator.GetStatelessManagedToUnmanaged(marshallerInfo, refKind),
                RefKind.Out => MarshallerShapeActivator.GetStatelessUnmanagedToManaged(marshallerInfo),
                RefKind.Ref or RefKind.RefReadOnlyParameter => MarshallerShapeActivator.GetStatelessBidirectional(marshallerInfo, refKind),
                _ => null
            };
        }

        if (marshallerInfo.MarshallerType.IsValueType)
        {
            // stateful
            return refKind switch
            {
                RefKind.In or RefKind.None => MarshallerShapeActivator.GetStatefulManagedToUnmanaged(marshallerInfo, refKind),
                RefKind.Out => MarshallerShapeActivator.GetStatefulUnmanagedToManaged(marshallerInfo),
                RefKind.Ref or RefKind.RefReadOnlyParameter => MarshallerShapeActivator.GetStatefulBidirectional(marshallerInfo, refKind),
                _ => null
            };
        }

        return null;
    }

    /// <summary>
    /// Returns all available marshaller modes for the specified marshaller type.
    /// </summary>
    private static MarshallerModeInfo[] GetModes(ITypeSymbol marshaller)
    {
        return marshaller.GetAttributes(Constants.CustomMarshallerAttributeFQN)
            .Select(GetModeFromAttribute)
            .Where(x => x != null)
            .ToArray();
    }

    /// <summary>
    /// Returns marshaller mode info for the specified [CustomMarshaller] attribute.
    /// </summary>
    private static MarshallerModeInfo GetModeFromAttribute(AttributeData attributeData)
    {
        var managedType = (ITypeSymbol)attributeData.ConstructorArguments[0].Value!;
        var mode = ModeForValue(attributeData.ConstructorArguments[1].Value!);
        var marshallerType = (ITypeSymbol)attributeData.ConstructorArguments[2].Value!;

        return new MarshallerModeInfo(managedType, mode, marshallerType);
    }
    
    private static MarshallerModeValue ModeForValue(object constant)
    {
        return constant is int number 
            ? (MarshallerModeValue)number 
            : MarshallerModeValue.Other;
    }

}
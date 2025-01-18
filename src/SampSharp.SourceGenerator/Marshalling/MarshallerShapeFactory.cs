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
    public IMarshallerShape? GetMarshallerShape(IMethodSymbol method, MarshalDirection direction)
    {
        if (method.ReturnsVoid)
        {
            return null;
        }

        var marshalUsing = method.GetReturnTypeAttribute(Constants.MarshalUsingAttributeFQN);
        var typeMarshaller = method.ReturnType.GetAttribute(Constants.NativeMarshallingAttributeFQN);
        
        var marshallerType = GetMarshallerTypeForParameterType(typeMarshaller, marshalUsing, method.ReturnType);

        // Always handle return parameter as "out"
        return GetMarshallerShape(marshallerType, method.ReturnType, RefKind.Out, direction);
    }

    /// <summary>
    /// Returns the marshaller shape for the specified <paramref name="parameter"/>.
    /// </summary>
    public IMarshallerShape? GetMarshallerShape(IParameterSymbol parameter, MarshalDirection direction)
    {
        var marshalUsing = parameter.GetAttribute(Constants.MarshalUsingAttributeFQN);
        var typeMarshaller = parameter.Type.GetAttribute(Constants.NativeMarshallingAttributeFQN);
        
        var marshallerType = GetMarshallerTypeForParameterType(typeMarshaller, marshalUsing, parameter.Type);

        return GetMarshallerShape(marshallerType, parameter.Type, parameter.RefKind, direction);
    }

    private static MarshalDirectionInfo GetDirectionInfo(MarshalDirection direction)
    {
        return direction switch
        {
            MarshalDirection.ManagedToUnmanaged => MarshalDirectionInfo.ManagedToUnmanaged,
            MarshalDirection.UnmanagedToManaged => MarshalDirectionInfo.UnmanagedToManaged,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    private static IMarshallerShape? GetMarshallerShape(ITypeSymbol? marshallerType, ITypeSymbol parameterType, RefKind refKind, MarshalDirection direction)
    {
        var dir = GetDirectionInfo(direction);

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

        // Select best fitting marshaller mode for the parameter
        var marshallerMode = refKind 
            switch
        {
            RefKind.In or RefKind.RefReadOnlyParameter or RefKind.None => filteredModes.FirstOrDefault(x => x.MarshalMode == dir.In),
            RefKind.Out => filteredModes.FirstOrDefault(x => x.MarshalMode == dir.Out),
            RefKind.Ref => filteredModes.FirstOrDefault(x => x.MarshalMode == dir.Ref),
            _ => null
        };

        var defaultInfo = filteredModes.FirstOrDefault(x => x.MarshalMode == MarshalMode.Default);

        //  Create a shape instance for the selected marshaller mode
        var shape = marshallerMode == null 
            ? null 
            : CreateMarshaller(marshallerMode, refKind, direction);

        if (shape == null && defaultInfo != null)
        {
            shape = CreateMarshaller(defaultInfo, refKind, direction);
        }

        return shape;
    }

    private ITypeSymbol? GetMarshallerTypeForParameterType(AttributeData? typeMarshallerAttrib, AttributeData? marshalUsingAttrib,
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

    private static IMarshallerShape? CreateMarshaller(CustomMarshallerInfo customMarshallerInfo, RefKind refKind, MarshalDirection direction)
    {
        var refDirection = GetDirectionForRefKind(refKind);

        // If the direction is unmanaged to managed, we need to invert the ref direction
        if (direction == MarshalDirection.UnmanagedToManaged)
        {
            refDirection = refDirection switch
            {
                ValueDirection.ManagedToNative => ValueDirection.NativeToManaged,
                ValueDirection.NativeToManaged => ValueDirection.ManagedToNative,
                _ => refDirection
            };
        }

        if (!refDirection.HasValue)
        {
            return null;
        }

        return MarshallerShapeActivator.Create(customMarshallerInfo, refKind, refDirection.Value, direction);
    }

    private static ValueDirection? GetDirectionForRefKind(RefKind refKind)
    {
        return refKind switch
        {
            RefKind.In or RefKind.RefReadOnlyParameter or RefKind.None => ValueDirection.ManagedToNative,
            RefKind.Out => ValueDirection.NativeToManaged,
            RefKind.Ref => ValueDirection.Bidirectional,
            _ => null
        };
    }
    
    /// <summary>
    /// Returns all available marshaller modes for the specified marshaller type.
    /// </summary>
    private static CustomMarshallerInfo[] GetModes(ITypeSymbol marshaller, ITypeSymbol forType)
    {
        return marshaller.GetAttributes(Constants.CustomMarshallerAttributeFQN)
            .Select(x => GetModeFromAttribute(x, forType))
            .WhereNotNull()
            .ToArray();
    }

    /// <summary>
    /// Returns marshaller mode info for the specified [CustomMarshaller] attribute.
    /// </summary>
    private static CustomMarshallerInfo? GetModeFromAttribute(AttributeData attributeData, ITypeSymbol forType)
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

        
        return new CustomMarshallerInfo(managedType, mode, marshallerType);
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

    private static MarshalMode ModeForValue(object constant)
    {
        return constant is int number 
            ? (MarshalMode)number 
            : MarshalMode.Other;
    }
}
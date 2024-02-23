using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator.Marshalling;

public static class WellKnownMarshallerTypes
{
    public static IMarshaller String { get; } = 
        new StatelessBidirectionalMarshallerStrategy(
            nativeTypeName: $"global::{Constants.StringViewFQN}",
            marshallerTypeName: "global::SashManaged.StringViewMarshaller",
            hasFree: true);

    public static IMarshaller Boolean { get; } = 
        new StatelessBidirectionalMarshallerStrategy(
            nativeTypeName: $"global::{Constants.BlittableBooleanFQN}",
            marshallerTypeName: "global::SashManaged.BooleanMarshaller",
            hasFree: false);

    public static IMarshaller GetMarshaller(IParameterSymbol parameterSymbol)
    {
        var refKind = parameterSymbol.RefKind;

        // Find marshaller type based on parameter or parameter type
        var marshalUsing = parameterSymbol.GetAttribute("System.Runtime.InteropServices.Marshalling.MarshalUsingAttribute");
        var typeMarshaller = parameterSymbol.Type.GetAttribute("System.Runtime.InteropServices.Marshalling.NativeMarshallingAttribute");

        var marshaller = typeMarshaller.ConstructorArguments[0].Value as ITypeSymbol;
        if (marshalUsing.ConstructorArguments.Length > 0)
        {
            if (marshalUsing.ConstructorArguments[0].Value is ITypeSymbol marshallerOverride)
            {
                marshaller = marshallerOverride;
            }
        }

        if (marshaller == null)
        {
            // TODO: Well known types?
            return null;
        }

        var modes = GetModes(marshaller);

        if (modes.Length == 0)
        {
            return null;
        }

        MarshallerModeInfo selected = null;
        var paramType = parameterSymbol.Type;
        switch (refKind)
        {
            case RefKind.In:
            case RefKind.None:
                selected = modes.FirstOrDefault(x => SymbolEquals(paramType, x.ManagedType) && x.Mode == MarshallerModeValue.ManagedToUnmanagedIn);
                break;
            case RefKind.Out:
                selected = modes.FirstOrDefault(x => SymbolEquals(paramType, x.ManagedType) && x.Mode == MarshallerModeValue.ManagedToUnmanagedOut);
                break;
            case RefKind.Ref:
            case RefKind.RefReadOnlyParameter:
                selected = modes.FirstOrDefault(x => SymbolEquals(paramType, x.ManagedType) && x.Mode == MarshallerModeValue.ManagedToUnmanagedRef);
                break;
        }

        selected ??= modes.FirstOrDefault(x => SymbolEquals(paramType, x.ManagedType) && x.Mode == MarshallerModeValue.Default);

        if (selected == null)
        {
            return null;
        }

        // TODO: if GetXXX return null marshaller, want to fallback to Default.
        if (selected.MarshallerType.IsStatic && !selected.MarshallerType.IsValueType)
        {
            // stateless
            switch (refKind)
            {
                case RefKind.In:
                case RefKind.None:
                    return GetStatelessManagedToUnmanged(selected);
                case RefKind.Out:
                    return GetStatelessUnmanagedToManaged(selected);
                    break;
                case RefKind.Ref:
                case RefKind.RefReadOnlyParameter:
                    return GetStatelessBidirectional(selected);
            }
            
        }
        else if (selected.MarshallerType.IsValueType)
        {
            // stateful
            #error TODO: return stateless marshaller strategy with correct configuration
        }

        return null;
    }

    private static bool SymbolEquals(ISymbol lhs, ISymbol rhs)
    {
        return SymbolEqualityComparer.Default.Equals(lhs, rhs);
    }

    private static string GetTypeString(ITypeSymbol symbol)
    {
        return symbol.SpecialType == SpecialType.None
            ? $"global::{symbol.ToDisplayString()}"
            : symbol.ToDisplayString();
    }

    private static IMethodSymbol GetMethod(ITypeSymbol type, string name, params ITypeSymbol[] argTypes)
    {
        return type
            .GetMembers(name)
            .OfType<IMethodSymbol>()
            .FirstOrDefault(x =>
            {
                if (x.Parameters.Length != argTypes.Length)
                {
                    return false;
                }

                return !argTypes.Where((t, i) => !SymbolEquals(x.Parameters[i].Type, t))
                    .Any();
            });

    }

    private static IMarshaller GetStatelessManagedToUnmanged(MarshallerModeInfo info)
    {
        var toUnmanaged = GetMethod(info.MarshallerType, "ConvertToUnmanaged", info.ManagedType);

        if (toUnmanaged == null)
        {
            return null;
        }

        var unmanagedType = toUnmanaged.ReturnType;

        var hasFree = GetMethod(info.MarshallerType, "Free", unmanagedType) != null;


        return new StatelessManagedToUnmanagedMarshallerStrategy(
            GetTypeString(unmanagedType), 
            GetTypeString(info.ManagedType), 
            hasFree);
    }
    
    private static IMarshaller GetStatelessUnmanagedToManaged(MarshallerModeInfo info)
    {
        var toManaged = info.MarshallerType
            .GetMembers("ConvertToManaged")
            .OfType<IMethodSymbol>()
            .SingleOrDefault();
            
        if (toManaged == null)
        {
            return null;
        }

        // TODO: check Parameters length
        var unmanagedType = toManaged.Parameters[0].Type;

        var hasFree = GetMethod(info.MarshallerType, "Free", unmanagedType) != null;


        return new StatelessUnmanagedToManagedMarshallerStrategy(
            GetTypeString(unmanagedType), 
            GetTypeString(info.ManagedType), 
            hasFree);
    }
    
    private static IMarshaller GetStatelessBidirectional(MarshallerModeInfo info)
    {
        var toManaged = info.MarshallerType
            .GetMembers("ConvertToManaged")
            .OfType<IMethodSymbol>()
            .SingleOrDefault();
           
        var toUnmanaged = GetMethod(info.MarshallerType, "ConvertToUnmanaged", info.ManagedType);
 
        if (toUnmanaged == null || toManaged == null)
        {
            return null;
        }

        // TODO: check Parameters length
        var unmanagedType = toManaged.Parameters[0].Type;

        if (!SymbolEquals(unmanagedType, toUnmanaged.ReturnType))
        {
            return null;
        }

        var hasFree = GetMethod(info.MarshallerType, "Free", unmanagedType) != null;


        return new StatelessBidirectionalMarshallerStrategy(
            GetTypeString(unmanagedType), 
            GetTypeString(info.ManagedType), 
            hasFree);
    }

    private static MarshallerModeInfo[] GetModes(ITypeSymbol marshaller)
    {
        return marshaller.GetAttributes("System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute")
            .Select(GetMode)
            .Where(x => x != null)
            .ToArray();
    }

    private static MarshallerModeInfo GetMode(AttributeData attributeData)
    {
        var managedType = (ITypeSymbol)attributeData.ConstructorArguments[0].Value;
        var mode = (MarshallerModeValue)(int)((TypedConstant)attributeData.ConstructorArguments[1].Value!).Value!;
        var marshallerType = (ITypeSymbol)attributeData.ConstructorArguments[2].Value;

        return new MarshallerModeInfo(managedType, mode, marshallerType);
    }

    private record MarshallerModeInfo(ITypeSymbol ManagedType, MarshallerModeValue Mode, ITypeSymbol MarshallerType);

    private enum MarshallerModeValue
    {
        Default,
        ManagedToUnmanagedIn,
        ManagedToUnmanagedRef,
        ManagedToUnmanagedOut,
        UnmanagedToManagedIn,
        UnmanagedToManagedRef,
        UnmanagedToManagedOut
    }
}
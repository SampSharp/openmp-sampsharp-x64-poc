using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using SashManaged.SourceGenerator.Marshalling.Stateful;
using SashManaged.SourceGenerator.Marshalling.Stateless;

namespace SashManaged.SourceGenerator.Marshalling;

public static class MarshallerShapeFactory
{
    public static IMarshallerShape GetMarshallerShape(IMethodSymbol symbol, WellKnownMarshallerTypes wellKnownMarshallerTypes)
    {
        if (symbol.ReturnsVoid)
        {
            return null;
        }

        // TODO: const for types
        var marshalUsing = symbol.GetReturnTypeAttribute("System.Runtime.InteropServices.Marshalling.MarshalUsingAttribute");
        var typeMarshaller = symbol.ReturnType.GetAttribute("System.Runtime.InteropServices.Marshalling.NativeMarshallingAttribute");
        
        // Always handle return parameter as "out"
        return GetMarshallerShape(wellKnownMarshallerTypes, typeMarshaller, marshalUsing, symbol.ReturnType, RefKind.Out);
    }

    public static IMarshallerShape GetMarshallerShape(IParameterSymbol symbol, WellKnownMarshallerTypes wellKnownMarshallerTypes)
    {
        var refKind = symbol.RefKind;
        var type = symbol.Type;

        var marshalUsing = symbol.GetAttribute("System.Runtime.InteropServices.Marshalling.MarshalUsingAttribute");
        var typeMarshaller = symbol.Type.GetAttribute("System.Runtime.InteropServices.Marshalling.NativeMarshallingAttribute");
        
        return GetMarshallerShape(wellKnownMarshallerTypes, typeMarshaller, marshalUsing, type, refKind);
    }

    private static IMarshallerShape GetMarshallerShape(WellKnownMarshallerTypes wellKnownMarshallerTypes, AttributeData typeMarshaller, AttributeData marshalUsing,
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
            var wk = wellKnownMarshallerTypes.Marshallers
                .FirstOrDefault(x => x.matcher(type) && x.marshaller != null)
                .marshaller;

            if (wk == null)
            {
                return null;
            }

            marshaller = wk;
        }

        var modes = GetModes(marshaller);
        if (modes.Length == 0)
        {
            return null;
        }

        MarshallerModeInfo selected = null;
        switch (refKind)
        {
            case RefKind.In:
            case RefKind.None:
                selected = modes.FirstOrDefault(x => SymbolEquals(type, x.ManagedType) && 
                                                     x.Mode == MarshallerModeValue.ManagedToUnmanagedIn);
                break;
            case RefKind.Out:
                selected = modes.FirstOrDefault(x => SymbolEquals(type, x.ManagedType) &&
                                                     x.Mode == MarshallerModeValue.ManagedToUnmanagedOut);
                break;
            case RefKind.Ref:
            case RefKind.RefReadOnlyParameter:
                selected = modes.FirstOrDefault(x => SymbolEquals(type, x.ManagedType) &&
                                                     x.Mode == MarshallerModeValue.ManagedToUnmanagedRef);
                break;
        }

        var defaultInfo = modes.FirstOrDefault(x => SymbolEquals(type, x.ManagedType) && 
                                                    x.Mode == MarshallerModeValue.Default);

        var shape = selected == null 
            ? null 
            : GetShapeForMarshaller(selected, refKind);

        if (shape == null && defaultInfo != null)
        {
            shape = GetShapeForMarshaller(defaultInfo, refKind);
        }

        return shape;
    }

    private static IMarshallerShape GetShapeForMarshaller(MarshallerModeInfo selected, RefKind refKind)
    {
        if (selected.MarshallerType.IsStatic && !selected.MarshallerType.IsValueType)
        {
            // stateless
            switch (refKind)
            {
                case RefKind.In:
                case RefKind.None:
                {
                    return GetStatelessManagedToUnmanged(selected);
                }
                case RefKind.Out:
                {
                    return GetStatelessUnmanagedToManaged(selected);
                }
                case RefKind.Ref:
                case RefKind.RefReadOnlyParameter:
                {
                    return GetStatelessBidirectional(selected);
                }
            }
        }
        else if (selected.MarshallerType.IsValueType)
        {
            // stateful
            switch (refKind)
            {
                case RefKind.In:
                case RefKind.None:
                {
                    return GetStatefulManagedToUnmanged(selected);
                }
                case RefKind.Out:
                {
                    return GetStatefulUnmanagedToManaged(selected);
                }
                case RefKind.Ref:
                case RefKind.RefReadOnlyParameter:
                {
                    return null;
                }
            }
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

    private static IMethodSymbol GetMethod(ITypeSymbol type, string name)
    {
        return GetMethod(type, name, Array.Empty<ITypeSymbol>());
    }

    private static IMethodSymbol GetMethod(ITypeSymbol type, string name, params ITypeSymbol[] argTypes)
    {
        // TODO: static/instance check
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

    private static IMethodSymbol GetMethod(ITypeSymbol type, string name, params Func<IParameterSymbol, bool>[] argChecks)
    {
        // TODO: static/instance check
        return type
            .GetMembers(name)
            .OfType<IMethodSymbol>()
            .FirstOrDefault(x =>
            {
                if (x.Parameters.Length != argChecks.Length)
                {
                    return false;
                }

                return !argChecks.Where((check, i) => !check(x.Parameters[i]))
                    .Any();
            });

    }

    private static IPropertySymbol GetProperty(ITypeSymbol type, string name, Func<ITypeSymbol, bool> check)
    {
        // TODO: static/instance check
        return type
            .GetMembers(name)
            .OfType<IPropertySymbol>()
            .FirstOrDefault(x => check(x.Type));

    }

    private static bool IsSpanByte(ITypeSymbol type)
    {
        if (type is not INamedTypeSymbol named || named.ToDisplayString() != "Sytem.Span<byte>")
        {
            return false;
        }

        return true;
    }

    private static IMarshallerShape GetStatefulUnmanagedToManaged(MarshallerModeInfo info)
    {
        var fromUnmanaged = GetMethod(info.MarshallerType, "FromUnmanaged", _ => true);
        var toManaged = GetMethod(info.MarshallerType, "ToManaged");
        var free = GetMethod(info.MarshallerType, "Free");
        
        if (fromUnmanaged == null || toManaged == null || free == null)
        {
            return null;
        }
        
        if (!SymbolEquals(toManaged.ReturnType, info.ManagedType))
        {
            return null;
        }
        
        var unmanagedType = fromUnmanaged.Parameters[0].Type;
        
        return new StatefulUnmanagedToManagedMarshallerShape(
            GetTypeString(unmanagedType), 
            GetTypeString(info.MarshallerType));
    }

    private static IMarshallerShape GetStatefulManagedToUnmanged(MarshallerModeInfo info)
    {
        var fromManaged = GetMethod(info.MarshallerType, "FromManaged", info.ManagedType);
        var toUnmanaged = GetMethod(info.MarshallerType, "ToUnmanaged");
        var free = GetMethod(info.MarshallerType, "Free");

        var fromManagedBuffer = GetMethod(info.MarshallerType, "FromManaged", x => SymbolEquals(x, info.ManagedType), x => IsSpanByte(x.Type));
        var bufferSize = GetProperty(info.MarshallerType, "BufferSize", x => x.SpecialType == SpecialType.System_Int32);

        var hasOnInvoked = GetMethod(info.MarshallerType, "OnInvoked") != null;

        if (toUnmanaged == null || free == null)
        {
            return null;
        }
        
        var unmanagedType = toUnmanaged.ReturnType;

        if (fromManaged != null)
        {
            // no buffer
            return new StatefulManagedToUnmanagedMarshallerShape(
                GetTypeString(unmanagedType), 
                GetTypeString(info.MarshallerType), 
                hasOnInvoked);
        }

        if (fromManagedBuffer != null && bufferSize != null)
        {
            // with buffer;
            return new StatefulManagedToUnmanagedWithBufferMarshallerShape(
                GetTypeString(unmanagedType), 
                GetTypeString(info.MarshallerType), 
                hasOnInvoked);
        }

        return null;
    }

    private static IMarshallerShape GetStatelessManagedToUnmanged(MarshallerModeInfo info)
    {
        var toUnmanaged = GetMethod(info.MarshallerType, "ConvertToUnmanaged", info.ManagedType);

        if (toUnmanaged == null)
        {
            return null;
        }

        var unmanagedType = toUnmanaged.ReturnType;

        var hasFree = GetMethod(info.MarshallerType, "Free", unmanagedType) != null;


        return new StatelessManagedToUnmanagedMarshallerShape(
            GetTypeString(unmanagedType), 
            GetTypeString(info.MarshallerType), 
            hasFree);
    }
    
    private static IMarshallerShape GetStatelessUnmanagedToManaged(MarshallerModeInfo info)
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


        return new StatelessUnmanagedToManagedMarshallerShape(
            GetTypeString(unmanagedType), 
            GetTypeString(info.MarshallerType), 
            hasFree);
    }
    
    private static IMarshallerShape GetStatelessBidirectional(MarshallerModeInfo info)
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

        if (toManaged.Parameters.Length != 1)
        {
            return null;
        }
        var unmanagedType = toManaged.Parameters[0].Type;

        if (!SymbolEquals(unmanagedType, toUnmanaged.ReturnType))
        {
            return null;
        }

        var hasFree = GetMethod(info.MarshallerType, "Free", unmanagedType) != null;


        return new StatelessBidirectionalMarshallerShape(
            GetTypeString(unmanagedType), 
            GetTypeString(info.MarshallerType), 
            hasFree);
    }

    private static MarshallerModeInfo[] GetModes(ITypeSymbol marshaller)
    {
        return marshaller.GetAttributes("System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute")
            .Select(GetMode)
            .Where(x => x != null)
            .ToArray();
    }

    private static MarshallerModeValue ModeForValue(object constant)
    {
        var str = constant.ToString();
        switch (str)
        {
            // TODO: is constant of type int?
            case "0":
                return MarshallerModeValue.Default;
            case "1":
                return MarshallerModeValue.ManagedToUnmanagedIn;
            case "2":
                return MarshallerModeValue.ManagedToUnmanagedRef;
            case "3":
                return MarshallerModeValue.ManagedToUnmanagedOut;
        }
        return MarshallerModeValue.Other;
    }

    private static MarshallerModeInfo GetMode(AttributeData attributeData)
    {
        var managedType = (ITypeSymbol)attributeData.ConstructorArguments[0].Value;
        var mode = ModeForValue(attributeData.ConstructorArguments[1].Value!);
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
        UnmanagedToManagedOut,
        Other
    }
}
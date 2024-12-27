using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using SashManaged.SourceGenerator.Marshalling.Stateful;
using SashManaged.SourceGenerator.Marshalling.Stateless;
using SashManaged.SourceGenerator.SyntaxFactories;

namespace SashManaged.SourceGenerator.Marshalling;

public static class MarshallerShapeActivator
{
    // The following value marshaller shapes are documented here
    // https://github.com/dotnet/runtime/blob/main/docs/design/libraries/LibraryImportGenerator/UserTypeMarshallingV2.md
    //
    // Stateless Managed->Unmanaged (TODO: implemented except for pinned reference)
    // Stateless Managed->Unmanaged with Caller-Allocated Buffer (implemented)
    // Stateless Unmanaged->Managed (implemented)
    // Stateless Unmanaged->Managed with Guaranteed Unmarshalling (TODO: not implemented)
    // Stateless Bidirectional (TODO: implemented without GU and pinning)
    // Stateful Managed->Unmanaged (TODO: implemented without pinning)
    // Stateful Managed->Unmanaged with Caller Allocated Buffer (TODO: implemented without pinning)
    // Stateful Unmanaged->Managed (implemented)
    // Stateful Unmanaged->Managed with Guaranteed Unmarshalling (TODO: not implemented)
    // Stateful Bidirectional (TODO: not implemented)
    //
    // TODO: No collection marshallers have been implemented (do we need them?)

    public static IMarshallerShape? GetStatefulUnmanagedToManaged(MarshallerModeInfo info)
    {
        var fromUnmanaged = GetMethod(info.MarshallerType, true, "FromUnmanaged", _ => true);
        var toManaged = GetMethod(info.MarshallerType, true, "ToManaged");
        var free = GetMethod(info.MarshallerType, true, "Free");
        
        if (fromUnmanaged == null || toManaged == null || free == null)
        {
            return null;
        }
        
        if (!SymbolEqualityComparer.Default.Equals(toManaged.ReturnType, info.ManagedType))
        {
            return null;
        }
        
        var unmanagedType = fromUnmanaged.Parameters[0].Type;
        
        return new StatefulUnmanagedToManagedMarshallerShape(
            GetTypeString(unmanagedType), 
            GetTypeString(info.MarshallerType));
    }

    public static IMarshallerShape? GetStatefulBidirectional(MarshallerModeInfo info)
    {
        // TODO implement
        return null;
    }

    public static IMarshallerShape? GetStatefulManagedToUnmanged(MarshallerModeInfo info)
    {
        var fromManaged = GetMethod(info.MarshallerType, true, "FromManaged", info.ManagedType);
        var toUnmanaged = GetMethod(info.MarshallerType, true, "ToUnmanaged");
        var free = GetMethod(info.MarshallerType, true, "Free");

        var fromManagedBuffer = GetMethod(info.MarshallerType, true, "FromManaged", x => SymbolEqualityComparer.Default.Equals(x.Type, info.ManagedType), x => IsSpanByte(x.Type));
        var bufferSize = GetProperty(info.MarshallerType, true, "BufferSize", x => x.SpecialType == SpecialType.System_Int32);

        var hasOnInvoked = GetMethod(info.MarshallerType, true, "OnInvoked") != null;

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

    public static IMarshallerShape? GetStatelessManagedToUnmanaged(MarshallerModeInfo info)
    {
        var toUnmanaged = GetMethod(info.MarshallerType, false, "ConvertToUnmanaged", info.ManagedType);
        
        var bufferSize = GetProperty(info.MarshallerType, true, "BufferSize", x => x.SpecialType == SpecialType.System_Int32);
       
        var toUnmanagedBuffer = bufferSize == null 
            ? null
            : GetMethod(info.MarshallerType, false, "ConvertToUnmanaged", x => SymbolEqualityComparer.Default.Equals(x.Type, info.ManagedType), x => IsSpanByte(x.Type));
      
        if (toUnmanaged != null)
        {
            var hasFree = GetMethod(info.MarshallerType, false, "Free", toUnmanaged.ReturnType) != null;

            return new StatelessManagedToUnmanagedMarshallerShape(
                GetTypeString(toUnmanaged.ReturnType), 
                GetTypeString(info.MarshallerType), 
                hasFree);
        }

        if (bufferSize != null && toUnmanagedBuffer != null)
        {
            var hasFree = GetMethod(info.MarshallerType, false, "Free", toUnmanagedBuffer.ReturnType) != null;

            return new StatelessManagedToUnmanagedWithCalledAllocatedBufferMarshallerShape(
                GetTypeString(toUnmanagedBuffer.ReturnType), 
                GetTypeString(info.MarshallerType), 
                hasFree);
        }

        return null;
    }
    
    public static IMarshallerShape? GetStatelessUnmanagedToManaged(MarshallerModeInfo info)
    {
        var toManaged = info.MarshallerType
            .GetMembers("ConvertToManaged")
            .OfType<IMethodSymbol>()
            .SingleOrDefault();
            
        if (toManaged == null || toManaged.Parameters.Length != 1)
        {
            return null;
        }

        var unmanagedType = toManaged.Parameters[0].Type;
        
        var hasFree = GetMethod(info.MarshallerType, false, "Free", unmanagedType) != null;


        return new StatelessUnmanagedToManagedMarshallerShape(
            GetTypeString(unmanagedType), 
            GetTypeString(info.MarshallerType), 
            hasFree);
    }
    
    public static IMarshallerShape? GetStatelessBidirectional(MarshallerModeInfo info)
    {
        var toManaged = info.MarshallerType
            .GetMembers("ConvertToManaged")
            .OfType<IMethodSymbol>()
            .SingleOrDefault();
           
        var toUnmanaged = GetMethod(info.MarshallerType, false, "ConvertToUnmanaged", info.ManagedType);
 
        if (toUnmanaged == null || toManaged == null)
        {
            return null;
        }

        if (toManaged.Parameters.Length != 1)
        {
            return null;
        }
        var unmanagedType = toManaged.Parameters[0].Type;

        if (!SymbolEqualityComparer.Default.Equals(unmanagedType, toUnmanaged.ReturnType))
        {
            return null;
        }

        var hasFree = GetMethod(info.MarshallerType, false, "Free", unmanagedType) != null;

        return new StatelessBidirectionalMarshallerShape(
            GetTypeString(unmanagedType), 
            GetTypeString(info.MarshallerType), 
            hasFree);
    }
    
    private static string GetTypeString(ITypeSymbol symbol)
    {
        return symbol.SpecialType == SpecialType.None
            ? $"global::{symbol.ToDisplayString()}"
            : symbol.ToDisplayString();
    }
    
    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name)
    {
        return GetMethod(type, stateful, name, Array.Empty<ITypeSymbol>());
    }

    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name, params ITypeSymbol[] argTypes)
    {
        return type
            .GetMembers(name)
            .OfType<IMethodSymbol>()
            .FirstOrDefault(x =>
            {
                if (stateful == x.IsStatic || x.Parameters.Length != argTypes.Length)
                {
                    return false;
                }

                return !argTypes.Where((t, i) => !SymbolEqualityComparer.Default.Equals(x.Parameters[i].Type, t))
                    .Any();
            });

    }

    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name, params Func<IParameterSymbol, bool>[] argChecks)
    {
        return type
            .GetMembers(name)
            .OfType<IMethodSymbol>()
            .FirstOrDefault(x =>
            {
                if (stateful == x.IsStatic || x.Parameters.Length != argChecks.Length)
                {
                    return false;
                }

                return !argChecks.Where((check, i) => !check(x.Parameters[i]))
                    .Any();
            });

    }

    private static IPropertySymbol? GetProperty(ITypeSymbol type, bool isStatic, string name, Func<ITypeSymbol, bool> check)
    {
        return type
            .GetMembers(name)
            .OfType<IPropertySymbol>()
            .FirstOrDefault(x => isStatic == x.IsStatic && check(x.Type));

    }

    private static bool IsSpanByte(ITypeSymbol type)
    {
        return type is INamedTypeSymbol named && named.ToDisplayString() == Constants.SpanOfBytesFQN;
    }

}
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using SashManaged.SourceGenerator.Marshalling.Shapes;
using SashManaged.SourceGenerator.Marshalling.Shapes.Stateful;
using SashManaged.SourceGenerator.Marshalling.Shapes.Stateless;

namespace SashManaged.SourceGenerator.Marshalling;

public static class MarshallerShapeActivator
{
    // The following value marshaller shapes are documented here
    // https://github.com/dotnet/runtime/blob/main/docs/design/libraries/LibraryImportGenerator/UserTypeMarshallingV2.md
    //
    // Stateless Managed->Unmanaged
    // Stateless Managed->Unmanaged with Caller-Allocated Buffer
    // Stateless Unmanaged->Managed
    // Stateless Unmanaged->Managed with Guaranteed Unmarshalling
    // Stateless Bidirectional
    // Stateful Managed->Unmanaged
    // Stateful Managed->Unmanaged with Caller Allocated Buffer
    // Stateful Unmanaged->Managed
    // Stateful Unmanaged->Managed with Guaranteed Unmarshalling
    // Stateful Bidirectional
    //
    // NOTE: No collection marshallers have been implemented but so far we don't need them.

    public static IMarshallerShape? GetStatefulUnmanagedToManaged(MarshallerModeInfo info)
    {
        var fromUnmanaged = GetMethod(info.MarshallerType, true, "FromUnmanaged", _ => true);
        var free = GetMethod(info.MarshallerType, true, "Free");
        var toManaged = GetMethod(info.MarshallerType, true, "ToManaged");
        var toManagedFinally = GetMethod(info.MarshallerType, true, "ToManagedFinally");

        if (fromUnmanaged == null || free == null)
        {
            return null;
        }

        var unmanagedType = fromUnmanaged.Parameters[0].Type;
        
        if (toManaged != null && SymbolEqualityComparer.Default.Equals(toManaged.ReturnType, info.ManagedType))
        {
            return new StatefulUnmanagedToManagedMarshallerShape(GetTypeString(unmanagedType), GetTypeString(info.MarshallerType));
        }

        if (toManagedFinally != null && SymbolEqualityComparer.Default.Equals(toManagedFinally.ReturnType, info.ManagedType))
        {
            return new StatefulUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(GetTypeString(unmanagedType), GetTypeString(info.MarshallerType));
        }

        return null;
    }

    public static IMarshallerShape? GetStatefulManagedToUnmanaged(MarshallerModeInfo info, RefKind refKind)
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
        
        var getPinnableReferenceStatic = GetMethod(info.MarshallerType, false, "GetPinnableReference", info.ManagedType);
        if (getPinnableReferenceStatic?.ReturnsByRef == true && refKind == RefKind.None)
        {
            return new StatelessManagedToUnmanagedWithPinnableReferenceMarshallerShape(
                GetTypeString(unmanagedType), 
                GetTypeString(info.MarshallerType));
        }

        var getPinnableReference = GetMethod(info.MarshallerType, true, "GetPinnableReference");
        if (getPinnableReference?.ReturnsByRef == false)
        {
            getPinnableReference = null;
        }
        if (fromManaged != null)
        {
            // no buffer
            return new StatefulManagedToUnmanagedMarshallerShape(
                GetTypeString(unmanagedType), 
                GetTypeString(info.MarshallerType), 
                hasOnInvoked, 
                getPinnableReference != null);
        }

        if (fromManagedBuffer != null && bufferSize != null)
        {
            // with buffer;
            return new StatefulManagedToUnmanagedWithBufferMarshallerShape(
                GetTypeString(unmanagedType), 
                GetTypeString(info.MarshallerType), 
                hasOnInvoked,
                getPinnableReference != null);
        }

        return null;
    }

    public static IMarshallerShape? GetStatefulBidirectional(MarshallerModeInfo info, RefKind refKind)
    {
        var inShape = GetStatefulManagedToUnmanaged(info, refKind);
        var outShape = GetStatefulUnmanagedToManaged(info);

        if(inShape == null || outShape == null)
        {
            return null;
        }

        return new BidirectionalMarshallerShape(inShape, outShape);
    }

    public static IMarshallerShape? GetStatelessManagedToUnmanaged(MarshallerModeInfo info, RefKind refKind)
    {
        var toUnmanaged = GetMethod(info.MarshallerType, false, "ConvertToUnmanaged", info.ManagedType);
        
        var bufferSize = GetProperty(info.MarshallerType, true, "BufferSize", x => x.SpecialType == SpecialType.System_Int32);
       
        var toUnmanagedBuffer = bufferSize == null 
            ? null
            : GetMethod(info.MarshallerType, false, "ConvertToUnmanaged", x => SymbolEqualityComparer.Default.Equals(x.Type, info.ManagedType), x => IsSpanByte(x.Type));
      
        if (toUnmanaged != null)
        {
            var getPinnableReference = GetMethod(info.MarshallerType, false, "GetPinnableReference", info.ManagedType);

            if (getPinnableReference?.ReturnsByRef == true && refKind == RefKind.None)
            {
                return new StatelessManagedToUnmanagedWithPinnableReferenceMarshallerShape(
                    GetTypeString(toUnmanaged.ReturnType), 
                    GetTypeString(info.MarshallerType));
            }

            var hasFree = GetMethod(info.MarshallerType, false, "Free", toUnmanaged.ReturnType) != null;
            return new StatelessManagedToUnmanagedMarshallerShape(GetTypeString(toUnmanaged.ReturnType), GetTypeString(info.MarshallerType), hasFree);
        }

        if (bufferSize != null && toUnmanagedBuffer != null)
        {
            var hasFree = GetMethod(info.MarshallerType, false, "Free", toUnmanagedBuffer.ReturnType) != null;

            return new StatelessManagedToUnmanagedWithCallerAllocatedBufferMarshallerShape(
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
          
        if (toManaged is { Parameters.Length: 1 })
        {
            var unmanagedType = toManaged.Parameters[0].Type;

            var hasFree = GetMethod(info.MarshallerType, false, "Free", unmanagedType) != null;


            return new StatelessUnmanagedToManagedMarshallerShape(GetTypeString(unmanagedType), GetTypeString(info.MarshallerType), hasFree);
        }
          
        var toManagedFinally = info.MarshallerType
            .GetMembers("ConvertToManagedFinally")
            .OfType<IMethodSymbol>()
            .SingleOrDefault();

        if (toManagedFinally is { Parameters.Length: 1 })
        {
            var unmanagedType = toManagedFinally.Parameters[0].Type;

            var hasFree = GetMethod(info.MarshallerType, false, "Free", unmanagedType) != null;

            return new StatelessUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(GetTypeString(unmanagedType), GetTypeString(info.MarshallerType), hasFree);
        }

        return null;
    }
    
    public static IMarshallerShape? GetStatelessBidirectional(MarshallerModeInfo info, RefKind refKind)
    {
        var inShape = GetStatelessManagedToUnmanaged(info, refKind);
        var outShape = GetStatelessUnmanagedToManaged(info);

        if(inShape == null || outShape == null)
        {
            return null;
        }

        return new BidirectionalMarshallerShape(inShape, outShape);
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
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Helpers;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;
using SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

namespace SampSharp.SourceGenerator.Marshalling;

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
        
        var fromUnmanaged = GetMethod(info.MarshallerType, true, ShapeConstants.MethodFromUnmanaged, _ => true);
        var free = GetMethod(info.MarshallerType, true, ShapeConstants.MethodFree);
        var toManaged = GetMethod(info.MarshallerType, true, ShapeConstants.MethodToManaged);
        var toManagedFinally = GetMethod(info.MarshallerType, true, ShapeConstants.MethodToManagedFinally);

        if (fromUnmanaged == null || free == null)
        {
            return null;
        }

        var unmanagedType = fromUnmanaged.Parameters[0].Type;
        
        if (toManaged != null && SymbolEqualityComparer.Default.Equals(toManaged.ReturnType, info.ManagedType))
        {
            return new StatefulUnmanagedToManagedMarshallerShape(unmanagedType, info.MarshallerType);
        }

        if (toManagedFinally != null && SymbolEqualityComparer.Default.Equals(toManagedFinally.ReturnType, info.ManagedType))
        {
            return new StatefulUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(unmanagedType, info.MarshallerType);
        }

        return null;
    }

    public static IMarshallerShape? GetStatefulManagedToUnmanaged(MarshallerModeInfo info, RefKind refKind)
    {
        var fromManaged = GetMethod(info.MarshallerType, true, ShapeConstants.MethodFromManaged, info.ManagedType);
        var toUnmanaged = GetMethod(info.MarshallerType, true, ShapeConstants.MethodToUnmanaged);
        var free = GetMethod(info.MarshallerType, true, ShapeConstants.MethodFree);

        var fromManagedBuffer = GetMethod(info.MarshallerType, true, ShapeConstants.MethodFromManaged, x => SymbolEqualityComparer.Default.Equals(x.Type, info.ManagedType), x => IsSpanByte(x.Type));
        var bufferSize = GetProperty(info.MarshallerType, true, ShapeConstants.PropertyBufferSize, x => x.SpecialType == SpecialType.System_Int32);

        var hasOnInvoked = GetMethod(info.MarshallerType, true, ShapeConstants.MethodOnInvoked) != null;

        if (toUnmanaged == null || free == null)
        {
            return null;
        }
        
        var unmanagedType = toUnmanaged.ReturnType;
        
        var getPinnableReferenceStatic = GetMethod(info.MarshallerType, false, ShapeConstants.MethodGetPinnableReference, info.ManagedType);
        if (getPinnableReferenceStatic?.ReturnsByRef == true && refKind == RefKind.None)
        {
            return new StatelessManagedToUnmanagedWithPinnableReferenceMarshallerShape(
                unmanagedType, 
                info.MarshallerType);
        }

        var getPinnableReference = GetMethod(info.MarshallerType, true, ShapeConstants.MethodGetPinnableReference);
        if (getPinnableReference?.ReturnsByRef == false)
        {
            getPinnableReference = null;
        }
        if (fromManaged != null)
        {
            // no buffer
            return new StatefulManagedToUnmanagedMarshallerShape(
                unmanagedType, 
                info.MarshallerType, 
                hasOnInvoked, 
                getPinnableReference != null);
        }

        if (fromManagedBuffer != null && bufferSize != null)
        {
            // with buffer;
            return new StatefulManagedToUnmanagedWithBufferMarshallerShape(
                unmanagedType, 
                info.MarshallerType, 
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
        var toUnmanaged = GetMethod(info.MarshallerType, false, ShapeConstants.MethodConvertToUnmanaged, info.ManagedType);
        
        var bufferSize = GetProperty(info.MarshallerType, true, ShapeConstants.PropertyBufferSize, x => x.SpecialType == SpecialType.System_Int32);
       
        var toUnmanagedBuffer = bufferSize == null 
            ? null
            : GetMethod(info.MarshallerType, false, ShapeConstants.MethodConvertToUnmanaged, x => SymbolEqualityComparer.Default.Equals(x.Type, info.ManagedType), x => IsSpanByte(x.Type));
      
        if (toUnmanaged != null)
        {
            var getPinnableReference = GetMethod(info.MarshallerType, false, ShapeConstants.MethodGetPinnableReference, info.ManagedType);

            if (getPinnableReference?.ReturnsByRef == true && refKind == RefKind.None)
            {
                return new StatelessManagedToUnmanagedWithPinnableReferenceMarshallerShape(
                    toUnmanaged.ReturnType, 
                    info.MarshallerType);
            }

            var hasFree = GetMethod(info.MarshallerType, false, ShapeConstants.MethodFree, toUnmanaged.ReturnType) != null;
            return new StatelessManagedToUnmanagedMarshallerShape(toUnmanaged.ReturnType, info.MarshallerType, hasFree);
        }

        if (bufferSize != null && toUnmanagedBuffer != null)
        {
            var hasFree = GetMethod(info.MarshallerType, false, ShapeConstants.MethodFree, toUnmanagedBuffer.ReturnType) != null;

            return new StatelessManagedToUnmanagedWithCallerAllocatedBufferMarshallerShape(
                toUnmanagedBuffer.ReturnType, 
                info.MarshallerType, 
                hasFree);
        }

        return null;
    }
    
    public static IMarshallerShape? GetStatelessUnmanagedToManaged(MarshallerModeInfo info)
    {
        var toManaged =GetMethod(info.MarshallerType, false, ShapeConstants.MethodConvertToManaged, x => true);
          
        
        if (toManaged is { Parameters.Length: 1 })
        {
            var unmanagedType = toManaged.Parameters[0].Type;
            var hasFree = GetMethod(info.MarshallerType, false, ShapeConstants.MethodFree, unmanagedType) != null;

            return new StatelessUnmanagedToManagedMarshallerShape(unmanagedType, info.MarshallerType, hasFree);
        }

        var toManagedFinally = GetMethod(info.MarshallerType, false, ShapeConstants.MethodConvertToManagedFinally, x => true);

        if (toManagedFinally is { Parameters.Length: 1 })
        {
            var unmanagedType = toManagedFinally.Parameters[0].Type;
            var hasFree = GetMethod(info.MarshallerType, false, ShapeConstants.MethodFree, unmanagedType) != null;

            return new StatelessUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(unmanagedType, info.MarshallerType, hasFree);
        }

        return null;
    }
    
    public static IMarshallerShape? GetStatelessBidirectional(MarshallerModeInfo info, RefKind refKind)
    {
        var inShape = GetStatelessManagedToUnmanaged(info, refKind);
        var outShape = GetStatelessUnmanagedToManaged(info);

        return inShape != null && outShape != null 
            ? new BidirectionalMarshallerShape(inShape, outShape)
            : null;
    }

    /// <summary>
    /// Gets a method without parameters.
    /// </summary>
    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name)
    {
        return GetMethod(type, stateful, name, Array.Empty<ITypeSymbol>());
    }

    /// <summary>
    /// Gets a method with the specified parameter types.
    /// </summary>
    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name, params ITypeSymbol[] paramTypes)
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

                return !paramTypes.Where((t, i) => !x.Parameters[i].Type.IsSame(t))
                    .Any();
            });

    }
    
    /// <summary>
    /// Gets a method with the specified parameter types.
    /// </summary>
    private static IMethodSymbol? GetMethod(ITypeSymbol type, bool stateful, string name, params Func<IParameterSymbol, bool>[] paramTypes)
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

                return !paramTypes.Where((check, i) => !check(x.Parameters[i]))
                    .Any();
            });

    }

    /// <summary>
    /// Gets a property with the specified type.
    /// </summary>
    private static IPropertySymbol? GetProperty(ITypeSymbol type, bool isStatic, string name, Func<ITypeSymbol, bool> propertyType)
    {
        return type
            .GetMembers(name)
            .OfType<IPropertySymbol>()
            .FirstOrDefault(x => isStatic == x.IsStatic && propertyType(x.Type));

    }

    private static bool IsSpanByte(ITypeSymbol type)
    {
        return type is INamedTypeSymbol named && named.ToDisplayString() == Constants.SpanOfBytesFQN;
    }

}
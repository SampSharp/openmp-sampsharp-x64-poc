using System;
using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;
using SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

namespace SampSharp.SourceGenerator.Marshalling;

[Flags]
public enum MarshallerShape
{
    None = 0x0,
    ToUnmanaged = 0x1,
    CallerAllocatedBuffer = 0x2,
    StatelessPinnableReference = 0x4,
    StatefulPinnableReference = 0x8,
    ToManaged = 0x10,
    GuaranteedUnmarshal = 0x20,
    Free = 0x40,
    OnInvoked = 0x80,
}

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

    public static IMarshallerShape? Create(CustomMarshallerInfo info, RefKind refKind, MarshallerShapeDirection direction, MarshalDirection mDirection)
    {
        var stateful = !info.MarshallerType.IsStatic;

        if (stateful && !info.MarshallerType.IsValueType)
        {
            return null;
        }

        var methods = MarshalInspector.GetMembers(info);

        return (direction, stateful) switch
        {
            (MarshallerShapeDirection.ManagedToNative, true) => CreateStatefulManagedToUnmanaged(info, refKind, mDirection, methods),
            (MarshallerShapeDirection.NativeToManaged, true) => CreateStatefulUnmanagedToManaged(info, mDirection, methods),
            (MarshallerShapeDirection.Bidirectional, true) => CreateStatefulBidirectional(info, refKind, mDirection, methods),
            (MarshallerShapeDirection.ManagedToNative, false) => CreateStatelessManagedToUnmanaged(info, refKind, mDirection, methods),
            (MarshallerShapeDirection.NativeToManaged, false) => CreateStatelessUnmanagedToManaged(info, mDirection, methods),
            (MarshallerShapeDirection.Bidirectional, false) => CreateStatelessBidirectional(info, refKind, mDirection, methods),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static IMarshallerShape? CreateStatefulUnmanagedToManaged(CustomMarshallerInfo info, MarshalDirection direction, MarshalMembers members)
    {
        if (members.StatefulFromUnmanagedMethod == null || members.StatefulFreeMethod == null)
        {
            return null;
        }

        var unmanagedType = members.StatefulFromUnmanagedMethod.Parameters[0].Type;
        
        if (members.StatefulToManagedMethod != null && SymbolEqualityComparer.Default.Equals(members.StatefulToManagedMethod.ReturnType, info.ManagedType))
        {
            return new StatefulUnmanagedToManagedMarshallerShape(unmanagedType, info.MarshallerType, direction);
        }

        if (members.StatefulToManagedFinallyMethod != null && SymbolEqualityComparer.Default.Equals(members.StatefulToManagedFinallyMethod.ReturnType, info.ManagedType))
        {
            return new StatefulUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(unmanagedType, info.MarshallerType, direction);
        }

        return null;
    }

    private static IMarshallerShape? CreateStatefulManagedToUnmanaged(CustomMarshallerInfo info, RefKind refKind, MarshalDirection direction, MarshalMembers members)
    {
        if (members.StatefulToUnmanagedMethod == null || members.StatefulFreeMethod == null)
        {
            return null;
        }
        
        var unmanagedType = members.StatefulToUnmanagedMethod.ReturnType;

        if (members.StatelessGetPinnableReferenceMethod?.ReturnsByRef == true && refKind == RefKind.None)
        {
            return new StatelessManagedToUnmanagedWithPinnableReferenceMarshallerShape(
                unmanagedType, 
                info.MarshallerType, direction);
        }

        if (members.StatefulFromManagedMethod != null)
        {
            // no buffer
            return new StatefulManagedToUnmanagedMarshallerShape(
                unmanagedType, 
                info.MarshallerType, 
                members.StatefulOnInvokedMethod != null, 
                members.StatefulGetPinnableReferenceMethod != null, direction);
        }

        if (members.StatefulFromManagedWithBufferMethod != null && members.BufferSizeProperty != null)
        {
            // with buffer;
            return new StatefulManagedToUnmanagedWithBufferMarshallerShape(
                unmanagedType, 
                info.MarshallerType, 
                members.StatefulOnInvokedMethod != null,
                members.StatefulGetPinnableReferenceMethod != null, direction);
        }

        return null;
    }

    private static IMarshallerShape? CreateStatefulBidirectional(CustomMarshallerInfo info, RefKind refKind, MarshalDirection direction, MarshalMembers members)
    {
        var inShape = CreateStatefulManagedToUnmanaged(info, refKind, direction, members);
        var outShape = CreateStatefulUnmanagedToManaged(info, direction,members);

        if(inShape == null || outShape == null)
        {
            return null;
        }

        return new BidirectionalMarshallerShape(inShape, outShape);
    }

    private static IMarshallerShape? CreateStatelessManagedToUnmanaged(CustomMarshallerInfo info, RefKind refKind, MarshalDirection direction, MarshalMembers members)
    {
        if (members.StatelessConvertToUnmanagedMethod != null)
        {
            if (members.StatelessGetPinnableReferenceMethod != null && refKind == RefKind.None)
            {
                return new StatelessManagedToUnmanagedWithPinnableReferenceMarshallerShape(
                    members.StatelessConvertToUnmanagedMethod.ReturnType, 
                    info.MarshallerType, direction);
            }


            return new StatelessManagedToUnmanagedMarshallerShape(members.StatelessConvertToUnmanagedMethod.ReturnType, info.MarshallerType, members.StatelessFreeMethod != null, direction);
        }

        if (members is { BufferSizeProperty: not null, StatelessConvertToUnmanagedWithBufferMethod: not null })
        {
            return new StatelessManagedToUnmanagedWithCallerAllocatedBufferMarshallerShape(
                members.StatelessConvertToUnmanagedWithBufferMethod.ReturnType, 
                info.MarshallerType, 
                members.StatelessFreeMethod != null, direction);
        }

        return null;
    }
    
    private static IMarshallerShape? CreateStatelessUnmanagedToManaged(CustomMarshallerInfo info, MarshalDirection direction, MarshalMembers members)
    {
        if (members.StatelessConvertToManagedMethod is { Parameters.Length: 1 } toManaged)
        {
            var unmanagedType = toManaged.Parameters[0].Type;

            return new StatelessUnmanagedToManagedMarshallerShape(unmanagedType, info.MarshallerType, members.StatelessFreeMethod != null, direction);
        }

        if (members.StatelessConvertToManagedFinallyMethod is { Parameters.Length: 1 } toManagedFinally)
        {
            var unmanagedType = toManagedFinally.Parameters[0].Type;

            return new StatelessUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(unmanagedType, info.MarshallerType, members.StatelessFreeMethod != null, direction);
        }

        return null;
    }
    
    private static IMarshallerShape? CreateStatelessBidirectional(CustomMarshallerInfo info, RefKind refKind, MarshalDirection direction, MarshalMembers members)
    {
        var inShape = CreateStatelessManagedToUnmanaged(info, refKind, direction, members);
        var outShape = CreateStatelessUnmanagedToManaged(info, direction, members);

        return inShape != null && outShape != null 
            ? new BidirectionalMarshallerShape(inShape, outShape)
            : null;
    }
    
}
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;
using SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

namespace SampSharp.SourceGenerator.Marshalling;

public static class MarshallerShapeActivator
{
    public static IMarshallerShape? Create(CustomMarshallerInfo info, RefKind refKind, ValueDirection valueDirection, MarshalDirection marshalDirection)
    {
        if (!info.IsValid)
        {
            return null;
        }

        var methods = MarshalInspector.GetMembers(info);

        return (direction: valueDirection, info.IsStateful) switch
        {
            (ValueDirection.ManagedToNative, true) => CreateStatefulManagedToUnmanaged(info, refKind, marshalDirection, methods),
            (ValueDirection.NativeToManaged, true) => CreateStatefulUnmanagedToManaged(info, marshalDirection, methods),
            (ValueDirection.Bidirectional, true) => CreateStatefulBidirectional(info, refKind, marshalDirection, methods),
            (ValueDirection.ManagedToNative, false) => CreateStatelessManagedToUnmanaged(info, refKind, marshalDirection, methods),
            (ValueDirection.NativeToManaged, false) => CreateStatelessUnmanagedToManaged(info, marshalDirection, methods),
            (ValueDirection.Bidirectional, false) => CreateStatelessBidirectional(info, refKind, marshalDirection, methods),
            _ => throw new ArgumentOutOfRangeException(nameof(valueDirection), valueDirection, null)
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
﻿using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core;

[CustomMarshaller(typeof(bool), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(bool), MarshalMode.UnmanagedToManagedOut, typeof(ManagedToNative))]
[CustomMarshaller(typeof(bool), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
[CustomMarshaller(typeof(bool), MarshalMode.UnmanagedToManagedIn, typeof(NativeToManaged))]
[CustomMarshaller(typeof(bool), MarshalMode.ManagedToUnmanagedRef, typeof(Bidirectional))]
[CustomMarshaller(typeof(bool), MarshalMode.UnmanagedToManagedRef, typeof(Bidirectional))]
public static class BooleanMarshaller
{
    public static class ManagedToNative
    {
        public static BlittableBoolean ConvertToUnmanaged(bool managed)
        {
            return managed;
        }
    }
    
    public static class NativeToManaged
    {
        public static bool ConvertToManaged(BlittableBoolean unmanaged)
        {
            return unmanaged;
        }
    }

    public static class Bidirectional
    {
        public static BlittableBoolean ConvertToUnmanaged(bool managed)
        {
            return managed;
        }
        public static bool ConvertToManaged(BlittableBoolean unmanaged)
        {
            return unmanaged;
        }
    }
}
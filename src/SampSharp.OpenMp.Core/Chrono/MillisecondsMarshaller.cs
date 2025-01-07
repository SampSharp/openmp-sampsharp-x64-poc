using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core;

[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedOut, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedIn, typeof(NativeToManaged))]
public static class MillisecondsMarshaller
{
    public static class ManagedToNative
    {
        public static Milliseconds ConvertToUnmanaged(TimeSpan managed)
        {
            return new Milliseconds((int)managed.TotalMilliseconds);
        }
    }
    public static class NativeToManaged
    {
        public static TimeSpan ConvertToManaged(Milliseconds unmanaged)
        {
            return unmanaged.AsTimeSpan();
        }
    }
}
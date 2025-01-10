using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core;

[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedOut, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedIn, typeof(NativeToManaged))]
public static class MicrosecondsMarshaller
{
    public static class ManagedToNative
    {
        public static Microseconds ConvertToUnmanaged(TimeSpan managed)
        {
            return new Microseconds((int)managed.TotalMicroseconds);
        }

        public static void Free(int a, int b)
        {

        }
    }
    public static class NativeToManaged
    {
        public static TimeSpan ConvertToManaged(Microseconds unmanaged)
        {
            return unmanaged.AsTimeSpan();
        }
    }
}
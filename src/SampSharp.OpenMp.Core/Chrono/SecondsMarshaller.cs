using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core.Chrono;

[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedOut, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedIn, typeof(NativeToManaged))]
public static class SecondsMarshaller
{
    public static class ManagedToNative
    {
        public static Seconds ConvertToUnmanaged(TimeSpan managed)
        {
            return new Seconds((int)managed.TotalSeconds);
        }
    }
    public static class NativeToManaged
    {
        public static TimeSpan ConvertToManaged(Seconds unmanaged)
        {
            return unmanaged.AsTimeSpan();
        }
    }
}
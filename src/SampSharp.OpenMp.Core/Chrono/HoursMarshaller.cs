using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core;

[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedOut, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedIn, typeof(NativeToManaged))]
public static class HoursMarshaller
{
    public static class ManagedToNative
    {
        public static Hours ConvertToUnmanaged(TimeSpan managed)
        {
            return new Hours((int)managed.TotalHours);
        }
    }
    public static class NativeToManaged
    {
        public static TimeSpan ConvertToManaged(Hours unmanaged)
        {
            return unmanaged.AsTimeSpan();
        }
    }
}
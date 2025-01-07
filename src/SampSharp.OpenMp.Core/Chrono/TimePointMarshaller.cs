using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core;

[CustomMarshaller(typeof(DateTimeOffset), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(DateTimeOffset), MarshalMode.UnmanagedToManagedOut, typeof(ManagedToNative))]
[CustomMarshaller(typeof(DateTimeOffset), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
[CustomMarshaller(typeof(DateTimeOffset), MarshalMode.UnmanagedToManagedIn, typeof(NativeToManaged))]
public static class TimePointMarshaller
{
    public static class ManagedToNative
    {
        public static TimePoint ConvertToUnmanaged(DateTimeOffset managed)
        {
            return TimePoint.FromDateTimeOffset(managed);
        }
    }
    public static class NativeToManaged
    {
        public static DateTimeOffset ConvertToManaged(TimePoint unmanaged)
        {
            return unmanaged.ToDateTimeOffset();
        }
    }
}
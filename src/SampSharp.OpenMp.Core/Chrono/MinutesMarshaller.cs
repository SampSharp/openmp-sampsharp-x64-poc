using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core;

[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedOut, typeof(ManagedToNative))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
[CustomMarshaller(typeof(TimeSpan), MarshalMode.UnmanagedToManagedIn, typeof(NativeToManaged))]
public static class MinutesMarshaller
{
    public static class ManagedToNative
    {
        public static Minutes ConvertToUnmanaged(TimeSpan managed)
        {
            return new Minutes((int)managed.TotalMinutes);
        }
    }
    public static class NativeToManaged
    {
        public static TimeSpan ConvertToManaged(Minutes unmanaged)
        {
            return unmanaged.AsTimeSpan();
        }
    }
}
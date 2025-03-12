using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using SampSharp.OpenMp.Core.Chrono;

namespace SampSharp.OpenMp.Core.Api;

[CustomMarshaller(typeof(BanEntry), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(BanEntry), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
public static unsafe class BanEntryMarshaller
{
    public static class ManagedToNative
    {
        public static int BufferSize { get; } = Marshal.SizeOf<Native>();

        public static BlittableStructRef<Native> ConvertToUnmanaged(BanEntry managed, Span<byte> callerAllocatedBuffer)
        {
            var native = ToNative(managed);

            
            var ptr = (nint)Unsafe.AsPointer(ref callerAllocatedBuffer.GetPinnableReference());
            Marshal.StructureToPtr(native, ptr, false);

            return new BlittableStructRef<Native>(ptr);
        }
        
        private static Native ToNative(BanEntry entry)
        {
            return new Native(new HybridString46(entry.Address), 
                TimePoint.FromDateTimeOffset(entry.Time), 
                new HybridString25(entry.Name),
                new HybridString32(entry.Reason));
        }
    }

    public static class NativeToManaged
    {
        public static BanEntry? ConvertToManaged(BlittableStructRef<Native> unmanaged)
        {
            if (!unmanaged.HasValue)
            {
                return null;
            }

            var native = unmanaged.GetValueOrDefault();

            return FromNative(native);
        }
        
        private static BanEntry FromNative(Native native)
        {
            return new BanEntry(native.AddressString.ToString(),
                native.Time.ToDateTimeOffset(),
                native.Name.ToString(),
                native.Reason.ToString());
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Native(HybridString46 addressString, TimePoint time, HybridString25 name, HybridString32 reason)
    {
        public readonly HybridString46 AddressString = addressString;
        public readonly TimePoint Time = time;
        public readonly HybridString25 Name = name;
        public readonly HybridString32 Reason = reason;
    }
}
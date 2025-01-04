using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core.Api;

[CustomMarshaller(typeof(AnimationData), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(AnimationData), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
public static unsafe class AnimationDataMarshaller
{
    public static class ManagedToNative
    {
        public static int BufferSize { get; } = Marshal.SizeOf<Native>();

        public static BlittableStructRef<Native> ConvertToUnmanaged(AnimationData managed, Span<byte> callerAllocatedBuffer)
        {
            var native = ToNative(managed);

            
            var ptr = (nint)Unsafe.AsPointer(ref callerAllocatedBuffer.GetPinnableReference());
            Marshal.StructureToPtr(native, ptr, false);

            return new BlittableStructRef<Native>(ptr);
        }
        
        private static Native ToNative(AnimationData managed)
        {
            return new Native(managed.Delta, managed.Loop, managed.LockX, managed.LockY, managed.Freeze, managed.Time, new HybridString16(managed.Library),
                new HybridString24(managed.Name));
        }
    }

    public static class NativeToManaged
    {
        public static AnimationData? ConvertToManaged(BlittableStructRef<Native> unmanaged)
        {
            if (unmanaged.IsNull)
            {
                return null;
            }

            var native = unmanaged.GetValue();

            return FromNative(native);
        }
        
        private static AnimationData FromNative(Native native)
        {
            return new AnimationData(native.delta, native.loop, native.lockX, native.lockY, native.freeze, native.time, native.lib.ToString(), native.name.ToString());
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Native(float delta, bool loop, bool lockX, bool lockY, bool freeze, uint time, HybridString16 lib, HybridString24 name)
    {
        public readonly float delta = delta;
        public readonly bool loop = loop;
        public readonly bool lockX = lockX;
        public readonly bool lockY = lockY;
        public readonly bool freeze = freeze;
        public readonly uint time = time;
        public readonly HybridString16 lib = lib;
        public readonly HybridString24 name = name;
    }
}
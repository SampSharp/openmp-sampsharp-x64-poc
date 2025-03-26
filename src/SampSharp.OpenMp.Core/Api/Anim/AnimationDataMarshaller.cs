using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Represents a marshaller entrypoint for marshalling <see cref="AnimationData"/> to its native counterpart.
/// </summary>
[CustomMarshaller(typeof(AnimationData), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(AnimationData), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
public static unsafe class AnimationDataMarshaller
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
            if (!unmanaged.HasValue)
            {
                return null;
            }

            var native = unmanaged.GetValueOrDefault();

            return FromNative(native);
        }
        
        private static AnimationData FromNative(Native native)
        {
            return new AnimationData(native.delta, native.loop, native.lockX, native.lockY, native.freeze, native.time, native.lib.ToString(), native.name.ToString());
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Native
    {
        [FieldOffset(0)]
        public float delta;
        [FieldOffset(4)]
        public bool loop;
        [FieldOffset(5)]
        public bool lockX;
        [FieldOffset(6)]
        public bool lockY;
        [FieldOffset(7)]
        public bool freeze;
        [FieldOffset(8)]
        public uint time;
        [FieldOffset(16)]
        public HybridString16 lib;
        [FieldOffset(40)]
        public HybridString24 name;

        public Native(float delta, bool loop, bool lockX, bool lockY, bool freeze, uint time, HybridString16 lib, HybridString24 name)
        {
            this.delta = delta;
            this.loop = loop;
            this.lockX = lockX;
            this.lockY = lockY;
            this.freeze = freeze;
            this.time = time;
            this.lib = lib;
            this.name = name;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
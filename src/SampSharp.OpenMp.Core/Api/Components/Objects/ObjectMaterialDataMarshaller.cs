using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core.Api;

[CustomMarshaller(typeof(ObjectMaterialData), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
public static class ObjectMaterialDataMarshaller
{
    public static class NativeToManaged
    {
        public static ObjectMaterialData? ConvertToManaged(BlittableStructRef<NativeObjMat> unmanaged)
        {
            if (unmanaged.IsNull)
            {
                return null;
            }

            var native = unmanaged.GetValue();

            return FromNative(native);
        }

        private static ObjectMaterialData FromNative(NativeObjMat native)
        {
            return new ObjectMaterialData(native.Model, native.MaterialSize, native.FontSize, native.Alignment, native.Bold, native.MaterialColour, native.BackgroundColour,
                native.TextOrTXD.ToString(), native.FontOrTexture.ToString(), native.Type, native.Used);
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public readonly struct NativeObjMat
    {
        [FieldOffset(0)] public readonly int Model;
        [FieldOffset(0)] public readonly byte MaterialSize;
        [FieldOffset(1)] public readonly byte FontSize;
        [FieldOffset(2)] public readonly byte Alignment;
        [FieldOffset(3)] public readonly BlittableBoolean Bold; // len = 1
        [FieldOffset(4)] public readonly Colour MaterialColour; // len = 4

        // TODO: alignment issues. check if this is correct
        [FieldOffset(8)] public readonly Colour BackgroundColour; // len = 4
        [FieldOffset(16)] public readonly HybridString32 TextOrTXD; // len = 40
        [FieldOffset(56)] public readonly HybridString32 FontOrTexture; // len = 40

        [FieldOffset(96)] public readonly MaterialType Type; // len = 1
        [FieldOffset(97)] public readonly BlittableBoolean Used; // len = 1

        // used to be:
        // [FieldOffset(12)] public readonly HybridString32 TextOrTXD; // len = 40
        // [FieldOffset(52)] public readonly HybridString32 FontOrTexture; // len = 40
        // [FieldOffset(92)] public readonly MaterialType Type; // len = 1
        // [FieldOffset(93)] public readonly BlittableBoolean Used; // len = 1
    }
}
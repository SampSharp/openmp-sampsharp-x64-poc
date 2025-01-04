using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core.Api;

[CustomMarshaller(typeof(ObjectMaterialData), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
public static class ObjectMaterialDataMarshaller
{
    public static class NativeToManaged
    {
        public static ObjectMaterialData? ConvertToManaged(BlittableStructRef<Native> unmanaged)
        {
            if (unmanaged.IsNull)
            {
                return null;
            }

            var native = unmanaged.GetValue();

            return FromNative(native);
        }

        private static ObjectMaterialData FromNative(Native native)
        {
            return new ObjectMaterialData(native.Model, native.MaterialSize, native.FontSize, native.Alignment, native.Bold, native.MaterialColour, native.BackgroundColour,
                native.TextOrTXD.ToString(), native.FontOrTexture.ToString(), native.Type, native.Used);
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public readonly struct Native
    {
        [FieldOffset(0)] public readonly int Model;
        [FieldOffset(0)] public readonly byte MaterialSize;
        [FieldOffset(1)] public readonly byte FontSize;
        [FieldOffset(2)] public readonly byte Alignment;
        [FieldOffset(3)] public readonly BlittableBoolean Bold;
        [FieldOffset(4)] public readonly Colour MaterialColour;

        [FieldOffset(8)] public readonly Colour BackgroundColour;
        [FieldOffset(12)] public readonly HybridString32 TextOrTXD;
        [FieldOffset(12 + 32 + Size.Length)] public readonly HybridString32 FontOrTexture;

        [FieldOffset(12 + (32 + Size.Length) * 2)]
        public readonly MaterialType Type;

        [FieldOffset(12 + (32 + Size.Length) * 2 + 1)]
        public readonly BlittableBoolean Used;
    }
}
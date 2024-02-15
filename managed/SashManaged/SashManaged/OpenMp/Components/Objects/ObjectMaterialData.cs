using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Explicit)]
public readonly struct ObjectMaterialData 
{
    public enum MaterialType : byte
    {
        None,
        Default,
        Text
    }

    [FieldOffset(0)] public readonly int Model;
    [FieldOffset(0)] public readonly byte MaterialSize;
    [FieldOffset(1)] public readonly byte FontSize;
    [FieldOffset(2)] public readonly byte Alignment;
    [FieldOffset(3)] public readonly byte Bold;//boolean
    [FieldOffset(4)] public readonly Colour MaterialColour;
    public Colour FontColour => MaterialColour;

    [FieldOffset(8)] public readonly Colour BackgroundColour;
    [FieldOffset(12)] public readonly HybridString32 TextOrTXD;
    [FieldOffset(12 + 32 + Size.Length)] public readonly HybridString32 FontOrTexture;
    
    [FieldOffset(12 + (32 + Size.Length) * 2)] public readonly MaterialType Type;
    [FieldOffset(12 + (32 + Size.Length) * 2 + 1)] public readonly byte Used; // boolean
}
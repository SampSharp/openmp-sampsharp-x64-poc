using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Size(nint value)
{
    public const int Length = 8; // 64-bits

    public nint Value { get; } = value;

    public int ToInt32()
    {
        return Value.ToInt32();
    }

    public static explicit operator int(Size value)
    {
        return value.ToInt32();
    }

    public static implicit operator Size(int value)
    {
        return new Size(value);
    }
}
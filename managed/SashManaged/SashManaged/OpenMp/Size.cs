using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Size(nint value)
{
    public nint Value { get; } = value;

    public static implicit operator Size(int value)
    {
        return new Size(value);
    }
}
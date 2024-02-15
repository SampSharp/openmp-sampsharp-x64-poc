using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PairInt
{
    public readonly int First;
    public readonly int Second;
}
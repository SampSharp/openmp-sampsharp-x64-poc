using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct GtaVector4
{
    public readonly float X;
    public readonly float Y;
    public readonly float Z;
    public readonly float W;
}
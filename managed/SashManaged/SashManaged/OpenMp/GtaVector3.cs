using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct GtaVector3
{
    public readonly float X;
    public readonly float Y;
    public readonly float Z;
}
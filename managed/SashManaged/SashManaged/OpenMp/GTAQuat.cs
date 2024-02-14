using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct GTAQuat
{
    public readonly float W;
    public readonly float X;
    public readonly float Y;
    public readonly float Z;
}
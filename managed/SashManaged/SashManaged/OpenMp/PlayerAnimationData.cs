using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerAnimationData
{
    public readonly ushort ID;
    public readonly ushort flags;
}
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerAnimationData
{
    public readonly ushort ID;
    public readonly ushort flags;
}
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerKeyData
{
    public readonly uint keys;
    public readonly ushort upDown;
    public readonly ushort leftRight;
}
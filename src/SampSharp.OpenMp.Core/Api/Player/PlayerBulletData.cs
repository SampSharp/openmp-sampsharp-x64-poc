using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerBulletData
{
    public readonly Vector3 origin;
    public readonly Vector3 hitPos;
    public readonly Vector3 offset;
    public readonly byte weapon;
    public readonly PlayerBulletHitType hitType;
    public readonly ushort hitID;
}
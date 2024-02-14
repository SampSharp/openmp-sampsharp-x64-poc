using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerBulletData
{
    public readonly GtaVector3 origin;
    public readonly GtaVector3 hitPos;
    public readonly GtaVector3 offset;
    public readonly byte weapon;
    public readonly PlayerBulletHitType hitType;
    public readonly ushort hitID;
}
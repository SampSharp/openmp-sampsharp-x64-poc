using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerAimData
{
    public readonly GtaVector3 camFrontVector;
    public readonly GtaVector3 camPos;
    public readonly float aimZ;
    public readonly float camZoom;
    public readonly float aspectRatio;
    public readonly PlayerWeaponState weaponState;
    public readonly byte camMode;
}
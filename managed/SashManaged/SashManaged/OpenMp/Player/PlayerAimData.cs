using System.Numerics;
using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerAimData
{
    public readonly Vector3 camFrontVector;
    public readonly Vector3 camPos;
    public readonly float aimZ;
    public readonly float camZoom;
    public readonly float aspectRatio;
    public readonly PlayerWeaponState weaponState;
    public readonly byte camMode;
}
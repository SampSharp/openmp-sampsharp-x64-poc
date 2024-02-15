using System.Numerics;
using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehicleModelInfo
{
    public readonly Vector3 Size;
    public readonly Vector3 FrontSeat;
    public readonly Vector3 RearSeat;
    public readonly Vector3 PetrolCap;
    public readonly Vector3 FrontWheel;
    public readonly Vector3 RearWheel;
    public readonly Vector3 MidWheel;
    public readonly float FrontBumperZ;
    public readonly float RearBumperZ;
}
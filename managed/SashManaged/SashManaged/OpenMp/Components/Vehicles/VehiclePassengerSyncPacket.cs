using System.Numerics;
using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehiclePassengerSyncPacket
{
    public readonly int PlayerID;
    public readonly int VehicleID;
    public readonly ushort DriveBySeatAdditionalKeyWeapon; // TODO: bit mask;
    /* bitmask DriveBySeatAdditionalKeyWeapon -> struct
    {
        byte SeatID : 6;
        byte DriveBy : 1;
        byte Cuffed : 1;
        byte WeaponID : 6;
        byte AdditionalKey : 2;
    }*/

    public readonly ushort Keys;

    public readonly Vector2 HealthArmour;
    public readonly ushort LeftRight;
    public readonly ushort UpDown;
    public readonly Vector3 Position;
};
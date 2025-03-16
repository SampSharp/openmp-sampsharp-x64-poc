using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehiclePassengerSyncPacket
{
    public readonly int PlayerID;
    public readonly int VehicleID;
    public readonly ushort DriveBySeatAdditionalKeyWeapon;

    public byte SeatId => (byte)(DriveBySeatAdditionalKeyWeapon & 0xb111111);
    public bool DriveBy => (DriveBySeatAdditionalKeyWeapon & 0b1000000) != 0;
    public bool Cuffed => (DriveBySeatAdditionalKeyWeapon & 0b10000000) != 0;
    public byte WeaponId => (byte)((DriveBySeatAdditionalKeyWeapon >> 8) & 0b111111);
    public byte AdditionalKey => (byte)(DriveBySeatAdditionalKeyWeapon >> 14);

    public readonly ushort Keys;

    public readonly Vector2 HealthArmour;
    public readonly ushort LeftRight;
    public readonly ushort UpDown;
    public readonly Vector3 Position;
};
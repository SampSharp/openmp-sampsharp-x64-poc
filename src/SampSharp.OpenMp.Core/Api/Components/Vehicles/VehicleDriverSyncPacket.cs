using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehicleDriverSyncPacket
{
    public readonly int PlayerID;
    public readonly ushort VehicleID;
    public readonly ushort LeftRight;
    public readonly ushort UpDown;
    public readonly ushort Keys;
    public readonly GTAQuat Rotation;
    public readonly Vector3 Position;
    public readonly Vector3 Velocity;
    public readonly float Health;
    public readonly Vector2 PlayerHealthArmour;
    public readonly byte Siren;
    public readonly byte LandingGear;
    public readonly ushort TrailerID;
    public readonly BlittableBoolean HasTrailer;
    public readonly byte AdditionalKeyWeapon;
    public readonly uint HydraThrustAngle;

    public byte WeaponID => (byte)(AdditionalKeyWeapon & 0b111111);
    public byte AdditionalKey => (byte)(AdditionalKeyWeapon >> 6);

    private unsafe float GetTrainSpeed()
    {
        var value = HydraThrustAngle;
        return *(float*)&value;
    }

    public float TrainSpeed => GetTrainSpeed();
};
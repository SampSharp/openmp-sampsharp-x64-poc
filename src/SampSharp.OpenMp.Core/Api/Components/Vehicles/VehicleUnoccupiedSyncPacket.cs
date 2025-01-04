using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehicleUnoccupiedSyncPacket
{
    public readonly int VehicleID;
    public readonly int PlayerID;
    public readonly byte SeatID;
    public readonly Vector3 Roll;
    public readonly Vector3 Rotation;
    public readonly Vector3 Position;
    public readonly Vector3 Velocity;
    public readonly Vector3 AngularVelocity;
    public readonly float Health;
};
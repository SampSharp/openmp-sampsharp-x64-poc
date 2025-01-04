using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehicleTrailerSyncPacket
{
    public readonly int VehicleID;
    public readonly int PlayerID;
    public readonly Vector3 Position;
    public readonly Vector4 Quat;
    public readonly Vector3 Velocity;
    public readonly Vector3 TurnVelocity;
};
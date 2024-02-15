using System.Numerics;
using System.Runtime.InteropServices;
using SashManaged.Chrono;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehicleSpawnData
{
    public readonly Seconds respawnDelay;
    public readonly int modelID;
    public readonly Vector3 position;
    public readonly float zRotation;
    public readonly int colour1;
    public readonly int colour2;
    public readonly bool siren;
    public readonly int interior;
}
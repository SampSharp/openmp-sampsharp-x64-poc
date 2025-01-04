using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

public readonly struct GangZonePos(Vector2 min, Vector2 max)
{
    public readonly Vector2 Min = min;
    public readonly Vector2 Max = max;
}
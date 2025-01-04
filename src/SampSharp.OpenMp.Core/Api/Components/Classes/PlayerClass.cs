using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

public readonly struct PlayerClass
{
    public readonly int Team;
    public readonly int Skin;
    public readonly Vector3 Spawn;
    public readonly float Angle;
    public readonly WeaponSlots Weapons;
}
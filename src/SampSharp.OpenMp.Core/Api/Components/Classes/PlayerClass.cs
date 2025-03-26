using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerClass
{
    public readonly int Team;
    public readonly int Skin;
    public readonly Vector3 Spawn;
    public readonly float Angle;
    public readonly WeaponSlots Weapons;

    public PlayerClass(int team, int skin, Vector3 spawn, float angle, WeaponSlots weapons)
    {
        Team = team;
        Skin = skin;
        Spawn = spawn;
        Angle = angle;
        Weapons = weapons;
    }
}
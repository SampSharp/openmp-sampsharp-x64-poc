using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct WeaponSlotData(byte id, int ammo)
{
    public readonly byte Id = id;

    public readonly int Ammo = ammo;
}
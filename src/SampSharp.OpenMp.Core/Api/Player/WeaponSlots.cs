using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct WeaponSlots
{
    public const int MAX_WEAPON_SLOTS = 13;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_WEAPON_SLOTS)]
    public readonly WeaponSlotData[] Data;

    public WeaponSlots(WeaponSlotData[] data)
    {
        if (data.Length != MAX_WEAPON_SLOTS)
        {
            throw new ArgumentException("Slot count should be MAX_WEAPON_SLOTS", nameof(data));
        }

        Data = data;
    }
}
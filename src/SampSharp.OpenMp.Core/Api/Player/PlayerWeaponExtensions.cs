namespace SampSharp.OpenMp.Core.Api;

public static class PlayerWeaponExtensions
{
    private static readonly string[] _names =
    [
        "Fist",
        "Brass Knuckles",
        "Golf Club",
        "Nite Stick",
        "Knife",
        "Baseball Bat",
        "Shovel",
        "Pool Cue",
        "Katana",
        "Chainsaw",
        "Dildo",
        "Dildo",
        "Vibrator",
        "Vibrator",
        "Flowers",
        "Cane",
        "Grenade",
        "Teargas",
        "Molotov Cocktail", // 18
        "Invalid",
        "Invalid",
        "Invalid",
        "Colt 45", // 22
        "Silenced Pistol",
        "Desert Eagle",
        "Shotgun",
        "Sawn-off Shotgun",
        "Combat Shotgun",
        "UZI",
        "MP5",
        "AK47",
        "M4",
        "TEC9",
        "Rifle",
        "Sniper Rifle",
        "Rocket Launcher",
        "Heat Seaker",
        "Flamethrower",
        "Minigun",
        "Satchel Explosives",
        "Bomb",
        "Spray Can",
        "Fire Extinguisher",
        "Camera",
        "Night Vision Goggles",
        "Thermal Goggles",
        "Parachute", // 46
        "Invalid",
        "Invalid",
        "Vehicle", // 49
        "Helicopter Blades", // 50
        "Explosion", // 51
        "Invalid",
        "Drowned", // 53
        "Splat"
    ];

    public static string GetName(this PlayerWeapon weapon)
    {
        var number = (int)weapon;

        if (number < 0 || number >= _names.Length)
        {
            return "Invalid";
        }

        return _names[number];
    }
}
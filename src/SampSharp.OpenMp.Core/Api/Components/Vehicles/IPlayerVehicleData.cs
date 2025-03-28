namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerVehicleData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerVehicleData
{
    public static UID ExtensionId => new(0xa960485be6c70fb2);

    public partial IVehicle GetVehicle();
    public partial void ResetVehicle();
    public partial int GetSeat();
    public partial bool IsInModShop();
    public partial bool IsInDriveByMode();
    public partial bool IsCuffed();
}
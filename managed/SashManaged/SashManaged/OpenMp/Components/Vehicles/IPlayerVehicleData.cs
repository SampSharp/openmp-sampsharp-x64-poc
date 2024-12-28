namespace SashManaged.OpenMp;

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
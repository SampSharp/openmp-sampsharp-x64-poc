namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IExtension))]
public readonly partial struct IPlayerPickupData
{
    public static UID ExtensionId => new(0x98376F4428D7B70B);

    public partial int ToLegacyID(int real);
    public partial int FromLegacyID(int legacy);
    public partial void ReleaseLegacyID(int legacy);
    public partial int ReserveLegacyID();
    public partial void SetLegacyID(int legacy, int real);
    public partial int ToClientID(int real);
    public partial int FromClientID(int legacy);
    public partial void ReleaseClientID(int legacy);
    public partial int ReserveClientID();
    public partial void SetClientID(int legacy, int real);
}
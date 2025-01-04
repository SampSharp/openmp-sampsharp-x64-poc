namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerGangZoneData
{
    public static UID ExtensionId => new(0xee8d8056b3351d11);

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
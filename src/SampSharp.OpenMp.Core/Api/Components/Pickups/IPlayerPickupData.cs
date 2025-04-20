namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerPickupData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerPickupData
{
    /// <inheritdoc />
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
namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerMenuData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerMenuData
{
    /// <inheritdoc />
    public static UID ExtensionId => new(0x01d8e934e9791b99);

    public partial byte GetMenuID();
    public partial void SetMenuID(byte id);
}
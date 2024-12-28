namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerMenuData
{
    public static UID ExtensionId => new(0x01d8e934e9791b99);

    public partial byte GetMenuID();
    public partial void SetMenuID(byte id);
}
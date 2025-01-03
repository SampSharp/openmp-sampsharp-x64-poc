namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerCustomModelsData
{
    public static UID ExtensionId => new(0xD3E2F572B38FB3F2);

    public partial uint GetCustomSkin();
    public partial void SetCustomSkin(uint skinModel);
    public partial bool SendDownloadUrl(string url);
}
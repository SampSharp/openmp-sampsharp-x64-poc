namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct INetworkQueryExtension
{
    public static UID ExtensionId => new(0xfd46e147ea474971);

    public partial bool AddRule(StringView rule, StringView value);
    public partial bool RemoveRule(StringView rule);
    public partial bool IsValidRule(StringView rule);
}
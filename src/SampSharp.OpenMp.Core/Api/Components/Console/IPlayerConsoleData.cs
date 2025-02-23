namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerConsoleData
{
    public static UID ExtensionId => new(0x9f8d20f2f471cbae);
    public partial bool HasConsoleAccess();
    public partial void SetConsoleAccessibility(bool set);
}
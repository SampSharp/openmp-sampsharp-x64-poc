namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerConsoleData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerConsoleData
{
    public static UID ExtensionId => new(0x9f8d20f2f471cbae);
    public partial bool HasConsoleAccess();
    public partial void SetConsoleAccessibility(bool set);
}
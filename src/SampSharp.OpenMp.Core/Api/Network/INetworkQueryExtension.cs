namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="INetworkQueryExtension" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct INetworkQueryExtension
{
    /// <inheritdoc />
    public static UID ExtensionId => new(0xfd46e147ea474971);

    public partial bool AddRule(string rule, string value);
    public partial bool RemoveRule(string rule);
    public partial bool IsValidRule(string rule);
}
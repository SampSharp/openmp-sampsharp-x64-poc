namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerRecordingData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerRecordingData
{
    /// <inheritdoc />
    public static UID ExtensionId => new(0x34DB532857286482);

    public partial void Start(PlayerRecordingType type, string file);
    public partial void Stop();
}
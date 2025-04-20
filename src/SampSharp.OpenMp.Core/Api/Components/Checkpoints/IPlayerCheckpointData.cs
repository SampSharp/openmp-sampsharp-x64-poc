namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerCheckpointData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerCheckpointData
{
    /// <inheritdoc />
    public static UID ExtensionId => new(0xbc07576aa3591a66);

    public partial ref IRaceCheckpointData GetRaceCheckpoint();
    public partial ref ICheckpointData GetCheckpoint();
}
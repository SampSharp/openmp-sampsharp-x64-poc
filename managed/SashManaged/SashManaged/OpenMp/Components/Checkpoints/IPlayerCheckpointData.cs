namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerCheckpointData
{
    public static UID ExtensionId => new(0xbc07576aa3591a66);

    public partial ref IRaceCheckpointData GetRaceCheckpoint();
    public partial ref ICheckpointData GetCheckpoint();
}
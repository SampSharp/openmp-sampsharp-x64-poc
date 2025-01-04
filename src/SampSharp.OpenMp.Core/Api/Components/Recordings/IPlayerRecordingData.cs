namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerRecordingData
{
    public static UID ExtensionId => new(0x34DB532857286482);

    public partial void Start(PlayerRecordingType type, string file);
    public partial void Stop();
}
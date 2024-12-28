namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerModelsEventHandler : IEventHandler2
{
    void OnPlayerFinishedDownloading(IPlayer player);
    bool OnPlayerRequestDownload(IPlayer player, ModelDownloadType type, uint checksum);
}
namespace SashManaged.OpenMp;

public interface IPlayerModelsEventHandler
{
    void OnPlayerFinishedDownloading(IPlayer player);
    bool OnPlayerRequestDownload(IPlayer player, ModelDownloadType type, uint checksum);
}
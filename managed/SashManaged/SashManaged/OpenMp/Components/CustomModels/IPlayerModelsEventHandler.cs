namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IPlayerModelsEventHandler
{
    void OnPlayerFinishedDownloading(IPlayer player);
    bool OnPlayerRequestDownload(IPlayer player, ModelDownloadType type, uint checksum);
}
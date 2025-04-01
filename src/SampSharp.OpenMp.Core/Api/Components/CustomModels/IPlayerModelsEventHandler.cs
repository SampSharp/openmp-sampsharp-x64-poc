namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="ICustomModelsComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerModelsEventHandler
{
    void OnPlayerFinishedDownloading(IPlayer player);
    bool OnPlayerRequestDownload(IPlayer player, ModelDownloadType type, uint checksum);
}
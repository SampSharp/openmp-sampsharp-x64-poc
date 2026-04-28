using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class CustomModelsSystem : DisposableSystem, IPlayerModelsEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public CustomModelsSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider,
        SampSharpEnvironment omp)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;

        var models = omp.Components.QueryComponent<ICustomModelsComponent>();
        if (!models.HasValue) return;
        AddDisposable(models.GetEventDispatcher().Add(this));
    }

    public void OnPlayerFinishedDownloading(IPlayer player)
    {
        _eventDispatcher.Invoke("OnPlayerFinishedDownloading", _entityProvider.GetEntity(player));
    }

    public bool OnPlayerRequestDownload(IPlayer player, ModelDownloadType type, uint checksum)
    {
        return _eventDispatcher.InvokeAs("OnPlayerRequestDownload", true,
            _entityProvider.GetEntity(player), (int)type, (int)checksum);
    }
}

using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class GangZoneSystem : DisposableSystem, IGangZoneEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public GangZoneSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, OpenMp omp)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;
        AddDisposable(omp.Components.QueryComponent<IGangZonesComponent>().GetEventDispatcher().Add(this));
    }

    public void OnPlayerEnterGangZone(IPlayer player, IGangZone zone)
    {
        _eventDispatcher.Invoke("OnPlayerEnterGangZone",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(zone));
    }

    public void OnPlayerLeaveGangZone(IPlayer player, IGangZone zone)
    {
        _eventDispatcher.Invoke("OnPlayerLeaveGangZone",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(zone));
    }

    public void OnPlayerClickGangZone(IPlayer player, IGangZone zone)
    {
        _eventDispatcher.Invoke("OnPlayerClickGangZone",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(zone));
    }
}
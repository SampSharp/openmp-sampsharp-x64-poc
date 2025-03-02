using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class GangZoneSystem : DisposableSystem, IGangZoneEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public GangZoneSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp omp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;
        AddDisposable(omp.Components.QueryComponent<IGangZonesComponent>().GetEventDispatcher().Add(this));
    }

    public void OnPlayerEnterGangZone(IPlayer player, IGangZone zone)
    {
        _eventService.Invoke("OnPlayerEnterGangZone",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(zone));
    }

    public void OnPlayerLeaveGangZone(IPlayer player, IGangZone zone)
    {
        _eventService.Invoke("OnPlayerLeaveGangZone",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(zone));
    }

    public void OnPlayerClickGangZone(IPlayer player, IGangZone zone)
    {
        _eventService.Invoke("OnPlayerClickGangZone",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(zone));
    }
}
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PickupSystem : DisposableSystem, IPickupEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PickupSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp omp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;
        AddDisposable(omp.Components.QueryComponent<IPickupsComponent>().GetEventDispatcher().Add(this));
    }
    
    public void OnPlayerPickUpPickup(IPlayer player, IPickup pickup)
    {
        _eventService.Invoke("OnPlayerPickUpPickup",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(pickup));
    }
}
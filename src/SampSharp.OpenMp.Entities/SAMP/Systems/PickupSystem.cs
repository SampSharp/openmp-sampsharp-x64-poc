using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PickupSystem : DisposableSystem, IPickupEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public PickupSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, SampSharpEnvironment omp)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;
        AddDisposable(omp.Components.QueryComponent<IPickupsComponent>().GetEventDispatcher().Add(this));
    }
    
    public void OnPlayerPickUpPickup(IPlayer player, IPickup pickup)
    {
        _eventDispatcher.Invoke("OnPlayerPickUpPickup",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(pickup));
    }
}
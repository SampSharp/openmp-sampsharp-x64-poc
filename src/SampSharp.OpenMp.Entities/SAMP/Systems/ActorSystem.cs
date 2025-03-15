using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class ActorSystem : DisposableSystem, IActorEventHandler
{
    private readonly IOmpEntityProvider _entityProvider;
    private readonly IEventDispatcher _eventDispatcher;

    public ActorSystem(IOmpEntityProvider entityProvider, IEventDispatcher eventDispatcher, OpenMp omp)
    {
        _entityProvider = entityProvider;
        _eventDispatcher = eventDispatcher;
        AddDisposable(omp.Components.QueryComponent<IActorsComponent>().GetEventDispatcher().Add(this));
    }

    public void OnPlayerGiveDamageActor(IPlayer player, IActor actor, float amount, uint weapon, SampSharp.OpenMp.Core.Api.BodyPart part)
    {
        _eventDispatcher.Invoke("OnPlayerGiveDamageActor",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(actor),
            amount, 
            weapon, 
            part);
    }

    public void OnActorStreamOut(IActor actor, IPlayer forPlayer)
    {
        _eventDispatcher.Invoke("OnActorStreamOut", _entityProvider.GetEntity(actor), _entityProvider.GetEntity(forPlayer));
    }

    public void OnActorStreamIn(IActor actor, IPlayer forPlayer)
    {
        _eventDispatcher.Invoke("OnActorStreamIn", _entityProvider.GetEntity(actor), _entityProvider.GetEntity(forPlayer));
    }
}
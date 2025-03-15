using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerDamageSystem : DisposableSystem, IPlayerDamageEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerDamageSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerDamageDispatcher().Add(this));
    }

    public void OnPlayerDeath(IPlayer player, IPlayer killer, int reason)
    {
        _eventDispatcher.Invoke("OnPlayerDeath", _entityProvider.GetEntity(player), _entityProvider.GetEntity(killer), reason);
    }

    public void OnPlayerTakeDamage(IPlayer player, IPlayer from, float amount, uint weapon, SampSharp.OpenMp.Core.Api.BodyPart part)
    {
        _eventDispatcher.Invoke("OnPlayerTakeDamage", _entityProvider.GetEntity(player), _entityProvider.GetEntity(from), amount, weapon, part);
    }

    public void OnPlayerGiveDamage(IPlayer player, IPlayer to, float amount, uint weapon, SampSharp.OpenMp.Core.Api.BodyPart part)
    {
        _eventDispatcher.Invoke("OnPlayerGiveDamage", _entityProvider.GetEntity(player), _entityProvider.GetEntity(to), amount, weapon, part);
    }
}
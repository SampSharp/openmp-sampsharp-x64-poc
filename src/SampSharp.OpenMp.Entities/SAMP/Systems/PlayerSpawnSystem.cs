using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerSpawnSystem : DisposableSystem, IPlayerSpawnEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerSpawnSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerSpawnDispatcher().Add(this));
    }

    public bool OnPlayerRequestSpawn(IPlayer player)
    {
        return _eventDispatcher.InvokeAs("OnPlayerRequestSpawn", true, _entityProvider.GetEntity(player));
    }

    public void OnPlayerSpawn(IPlayer player)
    {
        _eventDispatcher.Invoke("OnPlayerSpawn", _entityProvider.GetEntity(player));
    }
}
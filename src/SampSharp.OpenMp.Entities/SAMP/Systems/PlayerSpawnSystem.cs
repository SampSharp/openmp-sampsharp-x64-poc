using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerSpawnSystem : DisposableSystem, IPlayerSpawnEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerSpawnSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerSpawnDispatcher().Add(this));
    }

    public bool OnPlayerRequestSpawn(IPlayer player)
    {
        var result = _eventService.Invoke("OnPlayerRequestSpawn", _entityProvider.GetEntity(player));
        
        return EventHelper.IsSuccessResponse(result);
    }

    public void OnPlayerSpawn(IPlayer player)
    {
        _eventService.Invoke("OnPlayerSpawn", _entityProvider.GetEntity(player));
    }
}
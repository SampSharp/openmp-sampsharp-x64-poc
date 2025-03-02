using SampSharp.OpenMp.Core.Api;
using SampSharp.OpenMp.Core.Chrono;

namespace SampSharp.Entities.SAMP;

internal class PlayerUpdateSystem : DisposableSystem, IPlayerUpdateEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerUpdateSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerUpdateDispatcher().Add(this));
    }

    public bool OnPlayerUpdate(IPlayer player, TimePoint now)
    {
        var result = _eventService.Invoke("OnPlayerUpdate", _entityProvider.GetEntity(player), now);
        
        return EventHelper.IsSuccessResponse(result);
    }
}
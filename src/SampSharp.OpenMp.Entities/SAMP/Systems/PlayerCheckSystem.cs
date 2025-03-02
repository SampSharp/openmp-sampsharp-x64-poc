using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerCheckSystem : DisposableSystem, IPlayerCheckEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerCheckSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerCheckDispatcher().Add(this));
    }

    public void OnClientCheckResponse(IPlayer player, int actionType, int address, int results)
    {
        _eventService.Invoke("OnClientCheckResponse", _entityProvider.GetEntity(player), actionType, address, results);
    }
}
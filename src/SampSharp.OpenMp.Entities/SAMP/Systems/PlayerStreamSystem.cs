using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerStreamSystem : DisposableSystem, IPlayerStreamEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerStreamSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerStreamDispatcher().Add(this));
    }

    public void OnPlayerStreamIn(IPlayer player, IPlayer forPlayer)
    {
        _eventService.Invoke("OnPlayerStreamIn", _entityProvider.GetEntity(player), _entityProvider.GetEntity(forPlayer));
    }

    public void OnPlayerStreamOut(IPlayer player, IPlayer forPlayer)
    {
        _eventService.Invoke("OnPlayerStreamOut", _entityProvider.GetEntity(player), _entityProvider.GetEntity(forPlayer));
    }
}
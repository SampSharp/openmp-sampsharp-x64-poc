using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerChangeSystem : DisposableSystem, IPlayerChangeEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerChangeSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerChangeDispatcher().Add(this));
    }

    public void OnPlayerScoreChange(IPlayer player, int score)
    {
        _eventService.Invoke("OnPlayerScoreChange", _entityProvider.GetEntity(player), score);
    }

    public void OnPlayerNameChange(IPlayer player, string oldName)
    {
        _eventService.Invoke("OnPlayerNameChange", _entityProvider.GetEntity(player), oldName);
    }

    public void OnPlayerInteriorChange(IPlayer player, uint newInterior, uint oldInterior)
    {
        _eventService.Invoke("OnPlayerInteriorChange", _entityProvider.GetEntity(player), newInterior, oldInterior);
    }

    public void OnPlayerStateChange(IPlayer player, PlayerState newState, PlayerState oldState)
    {
        _eventService.Invoke("OnPlayerStateChange", _entityProvider.GetEntity(player), newState, oldState);
    }

    public void OnPlayerKeyStateChange(IPlayer player, uint newKeys, uint oldKeys)
    {
        _eventService.Invoke("OnPlayerKeyStateChange", _entityProvider.GetEntity(player), newKeys, oldKeys);
    }
}
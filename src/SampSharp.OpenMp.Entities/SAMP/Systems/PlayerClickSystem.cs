using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerClickSystem : DisposableSystem, IPlayerClickEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerClickSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerClickDispatcher().Add(this));
    }

    public void OnPlayerClickMap(IPlayer player, Vector3 pos)
    {
        _eventService.Invoke("OnPlayerClickMap", _entityProvider.GetEntity(player), pos);
    }

    public void OnPlayerClickPlayer(IPlayer player, IPlayer clicked, PlayerClickSource source)
    {
        _eventService.Invoke("OnPlayerClickPlayer", _entityProvider.GetEntity(player), clicked, source);
    }
}
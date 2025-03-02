using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerConnectSystem : DisposableSystem, IPlayerConnectEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerConnectSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerConnectDispatcher().Add(this));
    }

    public void OnIncomingConnection(IPlayer player, string ipAddress, ushort port)
    {
        _eventService.Invoke("OnIncomingConnection", _entityProvider.GetEntity(player), ipAddress, port);
    }

    public void OnPlayerConnect(IPlayer player)
    {
        _eventService.Invoke("OnPlayerConnect", _entityProvider.GetEntity(player));
    }

    public void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason)
    {
        _eventService.Invoke("OnPlayerDisconnect", _entityProvider.GetEntity(player), reason);
    }

    public void OnPlayerClientInit(IPlayer player)
    {
        _eventService.Invoke("OnPlayerClientInit", _entityProvider.GetEntity(player));
    }
}
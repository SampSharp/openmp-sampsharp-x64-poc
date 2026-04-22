using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerConnectSystem : DisposableSystem, IPlayerConnectEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;
    private readonly IEntityManager _entityManager;

    public PlayerConnectSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, IEntityManager entityManager, SampSharpEnvironment environment)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;
        _entityManager = entityManager;

        AddDisposable(environment.Core.GetPlayers().GetPlayerConnectDispatcher().Add(this));
    }

    public void OnIncomingConnection(IPlayer player, string ipAddress, ushort port)
    {
        _eventDispatcher.Invoke("OnIncomingConnection", _entityProvider.GetEntity(player), ipAddress, port);
    }

    public void OnPlayerConnect(IPlayer player)
    {
        _eventDispatcher.Invoke("OnPlayerConnect", _entityProvider.GetEntity(player));
    }

    public void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason)
    {
        var entity = _entityProvider.GetEntity(player);
        _eventDispatcher.Invoke("OnPlayerDisconnect", entity, reason);

        if (entity.IsEmpty) return;
        var component = _entityManager.GetComponent<Player>(entity);
        if (component is not { IsComponentAlive: true, IsDestroying: false }) return;

        player.TryGetExtension<ComponentExtension>()?.MarkOmpEntityDestroyed();
        component.Destroy();
    }

    public void OnPlayerClientInit(IPlayer player)
    {
        _eventDispatcher.Invoke("OnPlayerClientInit", _entityProvider.GetEntity(player));
    }
}
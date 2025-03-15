using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerCheckSystem : DisposableSystem, IPlayerCheckEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerCheckSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerCheckDispatcher().Add(this));
    }

    public void OnClientCheckResponse(IPlayer player, int actionType, int address, int results)
    {
        _eventDispatcher.Invoke("OnClientCheckResponse", _entityProvider.GetEntity(player), actionType, address, results);
    }
}
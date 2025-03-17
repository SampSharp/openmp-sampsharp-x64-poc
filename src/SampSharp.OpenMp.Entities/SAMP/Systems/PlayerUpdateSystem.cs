using SampSharp.OpenMp.Core.Api;
using SampSharp.OpenMp.Core.Chrono;

namespace SampSharp.Entities.SAMP;

internal class PlayerUpdateSystem : DisposableSystem, IPlayerUpdateEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerUpdateSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, SampSharpEnvironment environment)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;

        AddDisposable(environment.Core.GetPlayers().GetPlayerUpdateDispatcher().Add(this));
    }

    public bool OnPlayerUpdate(IPlayer player, TimePoint now)
    {
        return _eventDispatcher.InvokeAs("OnPlayerUpdate", true, _entityProvider.GetEntity(player), now);
    }
}
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class ClassSystem : DisposableSystem, IClassEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public ClassSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, SampSharpEnvironment omp)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;

        var classes = omp.Components.QueryComponent<IClassesComponent>();
        if (!classes.HasValue) return;
        AddDisposable(classes.GetEventDispatcher().Add(this));
    }

    public bool OnPlayerRequestClass(IPlayer player, uint classId)
    {
        return _eventDispatcher.InvokeAs("OnPlayerRequestClass", true,
            _entityProvider.GetEntity(player), (int)classId);
    }
}

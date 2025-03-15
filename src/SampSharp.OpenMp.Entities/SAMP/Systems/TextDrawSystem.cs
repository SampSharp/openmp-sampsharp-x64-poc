using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class TextDrawSystem : DisposableSystem, ITextDrawEventHandler
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOmpEntityProvider _entityProvider;

    public TextDrawSystem(IEventDispatcher eventDispatcher, IOmpEntityProvider entityProvider, OpenMp omp)
    {
        _eventDispatcher = eventDispatcher;
        _entityProvider = entityProvider;
        AddDisposable(omp.Components.QueryComponent<ITextDrawsComponent>().GetEventDispatcher().Add(this));
    }

    public void OnPlayerClickTextDraw(IPlayer player, ITextDraw td)
    {
        _eventDispatcher.Invoke("OnPlayerClickTextDraw",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(td));
    }

    public void OnPlayerClickPlayerTextDraw(IPlayer player, IPlayerTextDraw td)
    {
        _eventDispatcher.Invoke("OnPlayerClickPlayerTextDraw",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(td));
    }

    public bool OnPlayerCancelTextDrawSelection(IPlayer player)
    {
        return _eventDispatcher.InvokeAs("OnPlayerCancelTextDrawSelection", true,
            _entityProvider.GetEntity(player));
    }

    public bool OnPlayerCancelPlayerTextDrawSelection(IPlayer player)
    {
        return _eventDispatcher.InvokeAs("OnPlayerCancelPlayerTextDrawSelection", true,
            _entityProvider.GetEntity(player));
    }
}
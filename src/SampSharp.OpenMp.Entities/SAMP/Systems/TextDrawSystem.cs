using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class TextDrawSystem : DisposableSystem, ITextDrawEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public TextDrawSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp omp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;
        AddDisposable(omp.Components.QueryComponent<ITextDrawsComponent>().GetEventDispatcher().Add(this));
    }

    public void OnPlayerClickTextDraw(IPlayer player, ITextDraw td)
    {
        _eventService.Invoke("OnPlayerClickTextDraw",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(td));
    }

    public void OnPlayerClickPlayerTextDraw(IPlayer player, IPlayerTextDraw td)
    {
        _eventService.Invoke("OnPlayerClickPlayerTextDraw",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(td));
    }

    public bool OnPlayerCancelTextDrawSelection(IPlayer player)
    {
        var result = _eventService.Invoke("OnPlayerCancelTextDrawSelection",
            _entityProvider.GetEntity(player));
        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnPlayerCancelPlayerTextDrawSelection(IPlayer player)
    {
        var result = _eventService.Invoke("OnPlayerCancelPlayerTextDrawSelection",
            _entityProvider.GetEntity(player));
        return EventHelper.IsSuccessResponse(result);
    }
}
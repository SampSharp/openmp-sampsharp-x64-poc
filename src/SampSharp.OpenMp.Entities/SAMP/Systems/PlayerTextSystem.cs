using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerTextSystem : DisposableSystem, IPlayerTextEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerTextSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;
        AddDisposable(openMp.Core.GetPlayers().GetPlayerTextDispatcher().Add(this));
    }

    public bool OnPlayerText(IPlayer player, string message)
    {
        var result = _eventService.Invoke("OnPlayerText", _entityProvider.GetEntity(player), message);

        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnPlayerCommandText(IPlayer player, string message)
    {
        var result = _eventService.Invoke("OnPlayerCommandText", _entityProvider.GetEntity(player), message);

        return EventHelper.IsSuccessResponse(result);
    }
}
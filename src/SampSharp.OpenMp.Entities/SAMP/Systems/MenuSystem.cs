using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class MenuSystem : DisposableSystem, IMenuEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public MenuSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp omp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;
        AddDisposable(omp.Components.QueryComponent<IMenusComponent>().GetEventDispatcher().Add(this));
    }

    public void OnPlayerSelectedMenuRow(IPlayer player, byte row)
    {
        _eventService.Invoke("OnPlayerSelectedMenuRow",
            _entityProvider.GetEntity(player),
            row);
    }

    public void OnPlayerExitedMenu(IPlayer player)
    {
        _eventService.Invoke("OnPlayerExitedMenu",
            _entityProvider.GetEntity(player));
    }
}
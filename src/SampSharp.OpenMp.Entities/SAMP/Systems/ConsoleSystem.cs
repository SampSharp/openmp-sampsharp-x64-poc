using SampSharp.OpenMp.Core.Api;
using SampSharp.OpenMp.Core.RobinHood;

namespace SampSharp.Entities.SAMP;

internal class ConsoleSystem : ISystem, IConsoleEventHandler, IDisposable
{
    private readonly IOmpEntityProvider _entityProvider;
    private readonly IEventService _eventService;

    private IDisposable? _handler;

    public ConsoleSystem(IOmpEntityProvider entityProvider, IEventService eventService)
    {
        _entityProvider = entityProvider;
        _eventService = eventService;
    }

    [Event]
    public void OnGameModeInit(OpenMp omp)
    {
        _handler = omp.Components.QueryComponent<IConsoleComponent>().GetEventDispatcher().Add(this);
    }

    public bool OnConsoleText(string command, string parameters, ref ConsoleCommandSenderData sender)
    {
        var player = sender.Player.HasValue 
            ? _entityProvider.GetComponent(sender.Player.Value) 
            : null;
        var isCustom = sender.Sender == SampSharp.OpenMp.Core.Api.ConsoleCommandSender.Custom;
        var isConsole = sender.Sender == SampSharp.OpenMp.Core.Api.ConsoleCommandSender.Console;

        var result = _eventService.Invoke("OnConsoleText", command, parameters, new ConsoleCommandSender(player, isConsole, isCustom));

        return EventHelper.IsSuccessResponse(result);
    }

    public void OnRconLoginAttempt(IPlayer player, string password, bool success)
    {
        _eventService.Invoke("OnRconLoginAttempt", _entityProvider.GetEntity(player), password, success);
    }

    public void OnConsoleCommandListRequest(FlatHashSetStringView commands)
    {
        var collection = new ConsoleCommandCollection(commands);

        _eventService.Invoke("OnConsoleCommandListRequest", collection);
    }

    public void Dispose()
    {
        _handler?.Dispose();
        _handler = null;
    }
}
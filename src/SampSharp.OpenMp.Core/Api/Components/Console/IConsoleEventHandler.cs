using SampSharp.OpenMp.Core.RobinHood;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IConsoleComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IConsoleEventHandler
{
    bool OnConsoleText(string command, string parameters, ref ConsoleCommandSenderData sender);
    void OnRconLoginAttempt(IPlayer player, string password, bool success);
    void OnConsoleCommandListRequest(FlatHashSetStringView commands);
}
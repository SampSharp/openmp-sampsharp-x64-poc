namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IConsoleEventHandler
{
    bool OnConsoleText(string command, string parameters, ref ConsoleCommandSenderData sender);
    void OnRconLoginAttempt(IPlayer player, string password, bool success);
    void OnConsoleCommandListRequest(FlatHashSetStringView commands);
}
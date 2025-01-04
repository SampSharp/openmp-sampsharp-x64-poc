namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IConsoleEventHandler
{
    bool OnConsoleText(StringView command, StringView parameters, ref ConsoleCommandSenderData sender);
    // TODO: bool OnConsoleText(string command, string parameters, ref ConsoleCommandSenderData sender);
    void OnRconLoginAttempt(IPlayer player, StringView password, bool success);
    void OnConsoleCommandListRequest(FlatHashSetStringView commands);
}
namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IConsoleEventHandler
{
    bool OnConsoleText(StringView command, StringView parameters, ref ConsoleCommandSenderData sender);
    void OnRconLoginAttempt(IPlayer player, StringView password, bool success);
    void OnConsoleCommandListRequest(FlatHashSetStringView commands);
}
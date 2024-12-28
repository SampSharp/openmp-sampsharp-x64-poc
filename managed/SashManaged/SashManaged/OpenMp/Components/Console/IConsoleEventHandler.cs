namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IConsoleEventHandler : IEventHandler2
{
    bool OnConsoleText(StringView command, StringView parameters, ref ConsoleCommandSenderData sender);
    void OnRconLoginAttempt(IPlayer player, StringView password, bool success);
    void OnConsoleCommandListRequest(FlatHashSetStringView commands);
}
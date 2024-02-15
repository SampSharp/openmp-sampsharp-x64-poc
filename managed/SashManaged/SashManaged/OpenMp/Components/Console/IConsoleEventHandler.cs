namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IConsoleEventHandler
{
    // TODO: no ref bool OnConsoleText(StringView command, StringView parameters, ref ConsoleCommandSenderData sender);

    // TODO: codegen blittable return type not good void OnRconLoginAttempt(IPlayer player, StringView password, bool success);
    // TODO: void OnConsoleCommandListRequest(FlatHashSet<StringView>& commands);
}
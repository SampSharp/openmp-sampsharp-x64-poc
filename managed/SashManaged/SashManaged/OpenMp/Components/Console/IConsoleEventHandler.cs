﻿namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IConsoleEventHandler
{
    bool OnConsoleText(StringView command, StringView parameters, ref ConsoleCommandSenderData sender);

    void OnRconLoginAttempt(IPlayer player, StringView password, bool success);
    // TODO: void OnConsoleCommandListRequest(FlatHashSet<StringView>& commands);
}
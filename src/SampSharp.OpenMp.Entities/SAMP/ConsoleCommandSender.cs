namespace SampSharp.Entities.SAMP;

[EventParameter]
public record ConsoleCommandSender(Player? Player, bool IsConsole, bool IsCustom);
namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerTextEventHandler : IEventHandler2
{
    bool OnPlayerText(IPlayer player, StringView message);
    bool OnPlayerCommandText(IPlayer player, StringView message);
}
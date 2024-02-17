namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerTextEventHandler
{
    bool OnPlayerText(IPlayer player, StringView message);
    bool OnPlayerCommandText(IPlayer player, StringView message);
}
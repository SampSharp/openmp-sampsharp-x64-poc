namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerUpdateEventHandler
{
    bool OnPlayerUpdate(IPlayer player, TimePoint now);
}
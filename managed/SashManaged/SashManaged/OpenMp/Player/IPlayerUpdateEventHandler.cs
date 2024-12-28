namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerUpdateEventHandler : IEventHandler2
{
    bool OnPlayerUpdate(IPlayer player, TimePoint now);
}
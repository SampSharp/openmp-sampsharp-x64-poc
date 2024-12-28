namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IPlayerUpdateEventHandler
{
    bool OnPlayerUpdate(IPlayer player, TimePoint now);
}
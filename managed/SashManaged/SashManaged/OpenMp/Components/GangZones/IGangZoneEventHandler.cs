namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IGangZoneEventHandler : IEventHandler2
{
    void OnPlayerEnterGangZone(IPlayer player, IGangZone zone);
    void OnPlayerLeaveGangZone(IPlayer player, IGangZone zone);
    void OnPlayerClickGangZone(IPlayer player, IGangZone zone);
}
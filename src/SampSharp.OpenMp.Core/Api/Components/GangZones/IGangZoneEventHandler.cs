namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IGangZoneEventHandler
{
    void OnPlayerEnterGangZone(IPlayer player, IGangZone zone);
    void OnPlayerLeaveGangZone(IPlayer player, IGangZone zone);
    void OnPlayerClickGangZone(IPlayer player, IGangZone zone);
}
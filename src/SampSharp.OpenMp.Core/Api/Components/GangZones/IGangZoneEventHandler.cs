namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IGangZonesComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IGangZoneEventHandler
{
    void OnPlayerEnterGangZone(IPlayer player, IGangZone zone);
    void OnPlayerLeaveGangZone(IPlayer player, IGangZone zone);
    void OnPlayerClickGangZone(IPlayer player, IGangZone zone);
}
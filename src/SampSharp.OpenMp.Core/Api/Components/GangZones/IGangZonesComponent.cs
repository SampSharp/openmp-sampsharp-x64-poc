namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct IGangZonesComponent
{
    public static UID ComponentId => new(0xb3351d11ee8d8056);

    public partial IEventDispatcher<IGangZoneEventHandler> GetEventDispatcher();

    public partial IGangZone create(GangZonePos pos);

    public partial FlatPtrHashSet<IGangZone> GetCheckingGangZones();
    public partial void UseGangZoneCheck(IGangZone zone, bool enable);
    public partial int ToLegacyID(int real);
    public partial int FromLegacyID(int legacy);
    public partial void ReleaseLegacyID(int legacy);
    public partial int ReserveLegacyID();
    public partial void SetLegacyID(int legacy, int real);
}
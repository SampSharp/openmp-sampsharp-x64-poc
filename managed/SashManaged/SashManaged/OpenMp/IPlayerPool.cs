namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct IPlayerPool
{
    public partial IEventDispatcher_PlayerConnectEventHandler GetPlayerConnectDispatcher();
}
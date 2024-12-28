namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerConnectEventHandler : IEventHandler2
{
    void OnIncomingConnection(IPlayer player, StringView ipAddress, ushort port);
    void OnPlayerConnect(IPlayer player);
    void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason);
    void OnPlayerClientInit(IPlayer player);
}
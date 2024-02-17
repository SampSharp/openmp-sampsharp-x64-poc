namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface INetworkEventHandler
{
    void OnPeerConnect(IPlayer peer);
    void OnPeerDisconnect(IPlayer peer, PeerDisconnectReason reason);
}
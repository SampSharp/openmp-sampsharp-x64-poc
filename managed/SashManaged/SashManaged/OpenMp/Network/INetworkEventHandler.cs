namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface INetworkEventHandler : IEventHandler2
{
    void OnPeerConnect(IPlayer peer);
    void OnPeerDisconnect(IPlayer peer, PeerDisconnectReason reason);
}
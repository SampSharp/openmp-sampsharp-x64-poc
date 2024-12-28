namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface INetworkEventHandler
{
    void OnPeerConnect(IPlayer peer);
    void OnPeerDisconnect(IPlayer peer, PeerDisconnectReason reason);
}
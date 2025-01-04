namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface INetworkEventHandler
{
    void OnPeerConnect(IPlayer peer);
    void OnPeerDisconnect(IPlayer peer, PeerDisconnectReason reason);
}
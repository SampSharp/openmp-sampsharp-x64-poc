namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="INetwork.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface INetworkEventHandler
{
    void OnPeerConnect(IPlayer peer);
    void OnPeerDisconnect(IPlayer peer, PeerDisconnectReason reason);
}
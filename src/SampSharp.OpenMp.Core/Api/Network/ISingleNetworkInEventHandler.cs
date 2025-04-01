namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="INetwork.GetPerPacketInEventDispatcher"/> and <see cref="INetwork.GetPerRPCInEventDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface ISingleNetworkInEventHandler
{
    bool OnReceive(IPlayer peer, NetworkBitStream bs);
}
namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="INetwork.GetPerPacketInEventDispatcher" /> and <see cref="INetwork.GetPerRPCInEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface INetworkInEventHandler
{
    bool OnReceivePacket(IPlayer peer, int id, NetworkBitStream bs);
    bool OnReceiveRPC(IPlayer peer, int id, NetworkBitStream bs);
}
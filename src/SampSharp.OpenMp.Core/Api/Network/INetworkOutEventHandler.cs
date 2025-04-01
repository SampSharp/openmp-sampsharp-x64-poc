namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="INetwork.GetPerPacketOutEventDispatcher" /> and <see cref="INetwork.GetPerRPCOutEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface INetworkOutEventHandler
{
    bool OnSendPacket(IPlayer peer, int id, NetworkBitStream bs);
    bool OnSendRPC(IPlayer peer, int id, NetworkBitStream bs);
}
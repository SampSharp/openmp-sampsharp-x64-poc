namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="INetwork.GetPerPacketOutEventDispatcher"/> and <see cref="INetwork.GetPerRPCOutEventDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface ISingleNetworkOutEventHandler
{
    bool OnSend(IPlayer peer, NetworkBitStream bs);
}
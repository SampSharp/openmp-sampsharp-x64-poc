namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface INetworkOutEventHandler
{
    bool OnSendPacket(IPlayer peer, int id, NetworkBitStream bs);
    bool OnSendRPC(IPlayer peer, int id, NetworkBitStream bs);
}
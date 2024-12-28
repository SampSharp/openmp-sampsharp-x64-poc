namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface INetworkInEventHandler
{
    bool OnReceivePacket(IPlayer peer, int id, NetworkBitStream bs);
    bool OnReceiveRPC(IPlayer peer, int id, NetworkBitStream bs);
}
namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface INetworkInEventHandler
{
    bool OnReceivePacket(IPlayer peer, int id, NetworkBitStream bs);
    bool OnReceiveRPC(IPlayer peer, int id, NetworkBitStream bs) ;
}
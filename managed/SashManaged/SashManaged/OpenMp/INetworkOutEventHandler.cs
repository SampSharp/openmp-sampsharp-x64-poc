namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface INetworkOutEventHandler
{
    bool OnSendPacket(IPlayer peer, int id, NetworkBitStream bs);
    bool OnSendRPC(IPlayer peer, int id, NetworkBitStream bs);
}
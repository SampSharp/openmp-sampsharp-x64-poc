namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface INetworkOutEventHandler : IEventHandler2
{
    bool OnSendPacket(IPlayer peer, int id, NetworkBitStream bs);
    bool OnSendRPC(IPlayer peer, int id, NetworkBitStream bs);
}
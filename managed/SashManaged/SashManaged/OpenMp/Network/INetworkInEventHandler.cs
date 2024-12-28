namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface INetworkInEventHandler : IEventHandler2
{
    bool OnReceivePacket(IPlayer peer, int id, NetworkBitStream bs);
    bool OnReceiveRPC(IPlayer peer, int id, NetworkBitStream bs);
}
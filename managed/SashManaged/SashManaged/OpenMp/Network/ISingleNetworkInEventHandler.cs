namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface ISingleNetworkInEventHandler : IEventHandler2
{
    bool OnReceive(IPlayer peer, NetworkBitStream bs);
}
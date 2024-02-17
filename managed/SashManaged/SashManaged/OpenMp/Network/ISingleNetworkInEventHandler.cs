namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface ISingleNetworkInEventHandler
{
    bool OnReceive(IPlayer peer, NetworkBitStream bs);
}
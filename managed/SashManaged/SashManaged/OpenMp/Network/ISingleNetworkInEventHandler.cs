namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface ISingleNetworkInEventHandler
{
    bool OnReceive(IPlayer peer, NetworkBitStream bs);
}
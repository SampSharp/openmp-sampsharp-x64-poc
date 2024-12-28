namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface ISingleNetworkOutEventHandler
{
    bool OnSend(IPlayer peer, NetworkBitStream bs);
}
namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface ISingleNetworkOutEventHandler
{
    bool OnSend(IPlayer peer, NetworkBitStream bs);
}
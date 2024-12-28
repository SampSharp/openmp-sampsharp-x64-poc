namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface ISingleNetworkOutEventHandler : IEventHandler2
{
    bool OnSend(IPlayer peer, NetworkBitStream bs);
}
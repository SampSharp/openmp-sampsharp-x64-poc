namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface ISingleNetworkOutEventHandler
{
    bool OnSend(IPlayer peer, NetworkBitStream bs);
}
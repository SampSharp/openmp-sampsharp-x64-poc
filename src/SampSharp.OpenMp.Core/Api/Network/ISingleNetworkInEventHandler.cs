namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface ISingleNetworkInEventHandler
{
    bool OnReceive(IPlayer peer, NetworkBitStream bs);
}
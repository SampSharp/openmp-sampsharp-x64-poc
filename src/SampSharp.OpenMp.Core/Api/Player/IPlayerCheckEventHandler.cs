namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPlayerCheckEventHandler
{
    void OnClientCheckResponse(IPlayer player, int actionType, int address, int results);
}
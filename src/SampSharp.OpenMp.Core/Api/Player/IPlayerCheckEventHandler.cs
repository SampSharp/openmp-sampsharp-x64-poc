namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerCheckDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerCheckEventHandler
{
    void OnClientCheckResponse(IPlayer player, int actionType, int address, int results);
}
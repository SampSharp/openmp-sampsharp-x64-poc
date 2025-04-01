namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerStreamDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerStreamEventHandler
{
    void OnPlayerStreamIn(IPlayer player, IPlayer forPlayer);
    void OnPlayerStreamOut(IPlayer player, IPlayer forPlayer);
}
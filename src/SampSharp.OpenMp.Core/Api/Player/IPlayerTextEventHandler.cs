namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerTextDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerTextEventHandler
{
    bool OnPlayerText(IPlayer player, string message);
    bool OnPlayerCommandText(IPlayer player, string message);
}
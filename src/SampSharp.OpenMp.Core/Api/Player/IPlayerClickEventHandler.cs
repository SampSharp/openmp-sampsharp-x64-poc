using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerClickDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerClickEventHandler
{
    void OnPlayerClickMap(IPlayer player, Vector3 pos);
    void OnPlayerClickPlayer(IPlayer player, IPlayer clicked, PlayerClickSource source);
}
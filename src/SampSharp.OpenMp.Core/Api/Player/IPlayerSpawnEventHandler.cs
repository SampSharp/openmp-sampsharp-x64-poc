namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerSpawnDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerSpawnEventHandler
{
    bool OnPlayerRequestSpawn(IPlayer player);
    void OnPlayerSpawn(IPlayer player);
}
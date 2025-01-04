namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPlayerSpawnEventHandler
{
    bool OnPlayerRequestSpawn(IPlayer player);
    void OnPlayerSpawn(IPlayer player);
}
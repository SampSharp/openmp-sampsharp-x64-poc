namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IPlayerSpawnEventHandler
{
    bool OnPlayerRequestSpawn(IPlayer player);
    void OnPlayerSpawn(IPlayer player);
}
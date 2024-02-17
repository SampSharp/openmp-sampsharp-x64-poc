namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerSpawnEventHandler
{
    bool OnPlayerRequestSpawn(IPlayer player);
    void OnPlayerSpawn(IPlayer player);
}
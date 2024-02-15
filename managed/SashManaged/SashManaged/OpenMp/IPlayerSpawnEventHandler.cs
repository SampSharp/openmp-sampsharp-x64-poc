namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerSpawnEventHandler
{
    byte OnPlayerRequestSpawn(IPlayer player);
    void OnPlayerSpawn(IPlayer player);
}
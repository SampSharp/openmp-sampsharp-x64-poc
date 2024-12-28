namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerSpawnEventHandler : IEventHandler2
{
    bool OnPlayerRequestSpawn(IPlayer player);
    void OnPlayerSpawn(IPlayer player);
}
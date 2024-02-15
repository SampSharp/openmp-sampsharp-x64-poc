namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerShotEventHandler
{
    bool OnPlayerShotMissed(IPlayer player, PlayerBulletDataPtr bulletData);
    bool OnPlayerShotPlayer(IPlayer player, IPlayer target, PlayerBulletDataPtr bulletData);
    bool OnPlayerShotVehicle(IPlayer player, IVehicle target, PlayerBulletDataPtr bulletData);
    bool OnPlayerShotObject(IPlayer player, IObject target, PlayerBulletDataPtr bulletData);
    bool OnPlayerShotPlayerObject(IPlayer player, IPlayerObject target, PlayerBulletDataPtr bulletData);
}
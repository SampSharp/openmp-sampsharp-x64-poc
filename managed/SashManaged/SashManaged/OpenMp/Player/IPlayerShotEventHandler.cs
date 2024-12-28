namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerShotEventHandler : IEventHandler2
{
    bool OnPlayerShotMissed(IPlayer player, ref PlayerBulletData bulletData);
    bool OnPlayerShotPlayer(IPlayer player, IPlayer target, ref PlayerBulletData bulletData);
    bool OnPlayerShotVehicle(IPlayer player, IVehicle target, ref PlayerBulletData bulletData);
    bool OnPlayerShotObject(IPlayer player, IObject target, ref PlayerBulletData bulletData);
    bool OnPlayerShotPlayerObject(IPlayer player, IPlayerObject target, ref PlayerBulletData bulletData);
}
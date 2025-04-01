namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerShotDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerShotEventHandler
{
    bool OnPlayerShotMissed(IPlayer player, ref PlayerBulletData bulletData);
    bool OnPlayerShotPlayer(IPlayer player, IPlayer target, ref PlayerBulletData bulletData);
    bool OnPlayerShotVehicle(IPlayer player, IVehicle target, ref PlayerBulletData bulletData);
    bool OnPlayerShotObject(IPlayer player, IObject target, ref PlayerBulletData bulletData);
    bool OnPlayerShotPlayerObject(IPlayer player, IPlayerObject target, ref PlayerBulletData bulletData);
}
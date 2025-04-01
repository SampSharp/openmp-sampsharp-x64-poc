namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerDamageDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerDamageEventHandler
{
    void OnPlayerDeath(IPlayer player, IPlayer killer, int reason);
    void OnPlayerTakeDamage(IPlayer player, IPlayer from, float amount, uint weapon, BodyPart part);
    void OnPlayerGiveDamage(IPlayer player, IPlayer to, float amount, uint weapon, BodyPart part);
}
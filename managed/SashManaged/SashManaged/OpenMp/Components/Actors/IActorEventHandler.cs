namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IActorEventHandler : IEventHandler2
{
    void OnPlayerGiveDamageActor(IPlayer player, IActor actor, float amount, uint weapon, BodyPart part);
    void OnActorStreamOut(IActor actor, IPlayer forPlayer);
    void OnActorStreamIn(IActor actor, IPlayer forPlayer);
}
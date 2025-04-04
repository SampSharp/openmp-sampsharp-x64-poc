﻿namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IActorsComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IActorEventHandler
{
    void OnPlayerGiveDamageActor(IPlayer player, IActor actor, float amount, uint weapon, BodyPart part);
    void OnActorStreamOut(IActor actor, IPlayer forPlayer);
    void OnActorStreamIn(IActor actor, IPlayer forPlayer);
}
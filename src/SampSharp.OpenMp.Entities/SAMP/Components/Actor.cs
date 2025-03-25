﻿using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>Represents a component which provides the data and functionality of an actor.</summary>
public class Actor : WorldEntity
{
    private readonly IActorsComponent _actors;
    private readonly IActor _actor;

    /// <summary>Constructs an instance of Actor, should be used internally.</summary>
    protected Actor(IActorsComponent actors, IActor actor) : base((IEntity)actor)
    {
        _actors = actors;
        _actor= actor;
    }
  
    /// <summary>
    /// Gets a value indicating whether the open.mp entity counterpart has been destroyed.
    /// </summary>
    protected bool IsOmpEntityDestroyed => _actor.TryGetExtension<ComponentExtension>()?.IsOmpEntityDestroyed ?? true;

    /// <summary>Gets the facing angle of this actor.</summary>
    public virtual float Angle
    {
        get => float.RadiansToDegrees(MathHelper.GetZAngleFromRotationMatrix(Matrix4x4.CreateFromQuaternion(_actor.GetRotation())));
        set => Rotation = Quaternion.CreateFromAxisAngle(GtaVector.Up, float.DegreesToRadians(value));
    }

    /// <summary>
    /// Gets or sets the skin of the actor.
    /// </summary>
    public virtual int Skin
    {
        get => _actor.GetSkin();
        set => _actor.SetSkin(value);
    }

    /// <summary>Gets the health of this actor.</summary>
    public virtual float Health
    {
        get => _actor.GetHealth();
        set => _actor.SetHealth(value);
    }

    /// <summary>Gets or sets a value indicating whether this actor is invulnerable.</summary>
    public virtual bool IsInvulnerable
    {
        get => _actor.IsInvulnerable();
        set => _actor.SetInvulnerable(value);
    }

    /// <summary>Determines whether this actor is streamed in for the specified <paramref name="player" />.</summary>
    /// <param name="player">The player.</param>
    /// <returns>True if streamed in; False otherwise.</returns>
    public virtual bool IsStreamedIn(Player player)
    {
        return _actor.IsStreamedInForPlayer(player);
    }

    /// <summary>Applies the specified animation to this actor.</summary>
    /// <param name="library">The animation library from which to apply an animation.</param>
    /// <param name="name">The name of the animation to apply, within the specified library.</param>
    /// <param name="fDelta">The speed to play the animation.</param>
    /// <param name="loop">if set to <c>true</c> the animation will loop.</param>
    /// <param name="lockX">if set to <c>true</c> allow this Actor to move it's x-coordinate.</param>
    /// <param name="lockY">if set to <c>true</c> allow this Actor to move it's y-coordinate.</param>
    /// <param name="freeze">if set to <c>true</c> freeze this Actor at the end of the animation.</param>
    /// <param name="time">The amount of time (in milliseconds) to play the animation.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="library" /> or <paramref name="name" /> is null.</exception>
    public virtual void ApplyAnimation(string library, string name, float fDelta, bool loop, bool lockX, bool lockY, bool freeze, int time)
    {
        ArgumentNullException.ThrowIfNull(library);
        ArgumentNullException.ThrowIfNull(name);
        _actor.ApplyAnimation(new AnimationData(fDelta, loop, lockX, lockY, freeze, (uint)time, library, name));
    }

    /// <summary>Clear any animations applied to this actor.</summary>
    public virtual void ClearAnimations()
    {
        _actor.ClearAnimations();
    }
    
    /// <inheritdoc />
    protected override void OnDestroyComponent()
    {
        if (!IsOmpEntityDestroyed)
        {
            _actors.AsPool().Release(Id);
        }
    }
    
    /// <inheritdoc />
    public override string ToString()
    {
        return $"(Id: {Id})";
    }
    
    /// <summary>Performs an implicit conversion from <see cref="Actor"/> to <see cref="IActor"/>.</summary>
    public static implicit operator IActor(Actor actor)
    {
        return actor._actor;
    }
}
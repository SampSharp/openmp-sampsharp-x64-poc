using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>
/// Represents a component which exists in the 3D world.
/// </summary>
public abstract class WorldEntity : IdProvider
{
    private readonly IEntity _entity;

    protected WorldEntity(IEntity entity) : base((IIDProvider)entity)
    {
        _entity = entity;
    }

    /// <summary>
    /// Gets or sets the position of this component.
    /// </summary>
    public virtual Vector3 Position
    {
        get => _entity.GetPosition();
        set => _entity.SetPosition(value);
    }

    /// <summary>
    /// Gets or sets the rotation of this component.
    /// </summary>
    public virtual Quaternion Rotation
    {
        get => _entity.GetRotation();
        set => _entity.SetRotation(value);
    }

    /// <summary>
    /// Gets or sets the virtual world of this component.
    /// </summary>
    public virtual int VirtualWorld
    {
        get => _entity.GetVirtualWorld();
        set => _entity.SetVirtualWorld(value);
    }
}
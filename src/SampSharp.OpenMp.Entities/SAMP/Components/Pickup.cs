using System.Numerics;
using System.Reflection;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>Represents a component which provides the data and functionality of a pickup.</summary>
public class Pickup : Component
{
    private readonly IPickupsComponent _pickups;
    private readonly IPickup _pickup;

    /// <summary>Constructs an instance of Pickup, should be used internally.</summary>
    protected Pickup(IPickupsComponent pickups, IPickup pickup)
    {
        _pickups = pickups;
        _pickup = pickup;
    }
    
    /// <summary>
    /// Gets a value indicating whether the open.mp entity counterpart has been destroyed.
    /// </summary>
    protected bool IsOmpEntityDestroyed => _pickup.TryGetExtension<ComponentExtension>()?.IsOmpEntityDestroyed ?? true;

    /// <summary>Gets the identifier of this <see cref="Pickup"/>.</summary>
    public virtual int Id => _pickup.GetID();

    /// <summary>Gets the virtual world assigned to this <see cref="Pickup" />.</summary>
    public virtual int VirtualWorld => _pickup.GetVirtualWorld();

    /// <summary>Gets the model of this <see cref="Pickup" />.</summary>
    public virtual int Model => _pickup.GetModel();

    /// <summary>Gets the type of this <see cref="Pickup" />.</summary>
    public virtual int SpawnType => _pickup.GetPickupType();

    /// <summary>Gets the position of this <see cref="Pickup" />.</summary>
    public virtual Vector3 Position => _pickup.GetPosition();
    
    protected override void OnDestroyComponent()
    {
        if (!IsOmpEntityDestroyed)
        {
            _pickups.AsPool().Release(Id);
        }
    }

    public override string ToString()
    {
        return $"(Id: {Id}, Model: {Model})";
    }
    
    public static implicit operator IPickup(Pickup pickup)
    {
        return pickup._pickup;
    }
}
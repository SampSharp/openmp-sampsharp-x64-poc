using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>Represents a component which provides the data and functionality of a gang zone.</summary>
public class GangZone : Component
{
    private readonly IGangZonesComponent _gangZonesComponent;
    private readonly IGangZone _gangZone;

    /// <summary>Constructs an instance of GangZone, should be used internally.</summary>
    protected GangZone(IGangZonesComponent gangZonesComponent, IGangZone gangZone, Vector2 min, Vector2 max)
    {
        _gangZone = gangZone;
        _gangZonesComponent = gangZonesComponent;
        Min = min; 
        Max = max;
    }

    /// <summary>
    /// Gets the native open.mp entity counterpart.
    /// </summary>
    public IGangZone Native => _gangZone;

    /// <summary>
    /// Gets a value indicating whether the open.mp entity counterpart has been destroyed.
    /// </summary>
    protected bool IsOmpEntityDestroyed => _gangZone.TryGetExtension<ComponentExtension>()?.IsOmpEntityDestroyed ?? true;

    /// <summary>Gets the identifier of this <see cref="GangZone"/>.</summary>
    public virtual int Id => _gangZone.GetID();

    /// <summary>Gets the minimum position of this <see cref="GangZone"/>.</summary>
    public virtual Vector2 Min { get; }

    /// <summary>Gets the maximum position of this <see cref="GangZone"/>.</summary>
    public virtual Vector2 Max { get; }

    /// <summary>Gets the minimum x value for this <see cref="GangZone" />.</summary>
    public virtual float MinX => Min.X;

    /// <summary>Gets the minimum y value for this <see cref="GangZone" />.</summary>
    public virtual float MinY => Min.Y;

    /// <summary>Gets the maximum x value for this <see cref="GangZone" />.</summary>
    public virtual float MaxX => Max.X;

    /// <summary>Gets the maximum y value for this <see cref="GangZone" />.</summary>
    public virtual float MaxY => Max.Y;

    /// <summary>Gets or sets the color of this <see cref="GangZone" />.</summary>
    public virtual Colour Color { get; set; }

    /// <summary>Shows this <see cref="GangZone" />.</summary>
    public virtual void Show()
    {
        foreach (var player in Manager.GetComponents<Player>())
        {
            Show(player);
        }
    }

    /// <summary>Shows this <see cref="GangZone" /> to the specified <paramref name="player" />.</summary>
    /// <param name="player">The player.</param>
    public virtual void Show(Player player)
    {
        var clr = Color;
        _gangZone.ShowForPlayer(player.Native, ref clr);
    }

    /// <summary>Hides this <see cref="GangZone" />.</summary>
    public virtual void Hide()
    {
        foreach (var player in Manager.GetComponents<Player>())
        {
            Hide(player);
        }
    }

    /// <summary>Hides this <see cref="GangZone" /> for the specified <paramref name="player" />.</summary>
    /// <param name="player">The player.</param>
    public virtual void Hide(Player player)
    {
        _gangZone.HideForPlayer(player.Native);
    }

    /// <summary>Flashes this <see cref="GangZone" />.</summary>
    /// <param name="color">The color.</param>
    public virtual void Flash(Colour color)
    {
        foreach (var player in Manager.GetComponents<Player>())
        {
            Flash(player, color);
        }
    }

    /// <summary>Flashes this <see cref="GangZone" /> for the specified <paramref name="player" />.</summary>
    /// <param name="player">The player.</param>
    public virtual void Flash(Player player)
    {
        Flash(player, new Colour());
    }

    /// <summary>Flashes this <see cref="GangZone" /> for the specified <paramref name="player" />.</summary>
    /// <param name="player">The player.</param>
    /// <param name="color">The color.</param>
    public virtual void Flash(Player player, Colour color)
    {
        var clr = color;
        _gangZone.FlashForPlayer(player.Native, ref clr);
    }

    /// <summary>Stops this <see cref="GangZone" /> from flash.</summary>
    public virtual void StopFlash()
    {
        foreach (var player in Manager.GetComponents<Player>())
        {
            StopFlash(player);
        }
    }

    /// <summary>Stops this <see cref="GangZone" /> from flash for the specified player.</summary>
    /// <param name="player">The player.</param>
    public virtual void StopFlash(Player player)
    {
        _gangZone.StopFlashForPlayer(player.Native);
    }

    /// <inheritdoc />
    protected override void OnDestroyComponent()
    {
        if (!IsOmpEntityDestroyed)
        {
            _gangZonesComponent.AsPool().Release(Id);
        }
    }
}
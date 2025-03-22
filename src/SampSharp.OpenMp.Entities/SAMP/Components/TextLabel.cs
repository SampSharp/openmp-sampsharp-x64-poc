using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>Represents a component which provides the data and functionality of a 3D text label.</summary>
public class TextLabel : WorldEntity
{
    private readonly IOmpEntityProvider _entityProvider;
    private readonly ITextLabelsComponent _textLabels;
    private readonly ITextLabel _textLabel;

    /// <summary>Constructs an instance of TextLabel, should be used internally.</summary>
    protected TextLabel(IOmpEntityProvider entityProvider, ITextLabelsComponent textLabels, ITextLabel textLabel) : base((IEntity)textLabel)
    {
        _entityProvider = entityProvider;
        _textLabels = textLabels;
        _textLabel = textLabel;
    }
    
    /// <summary>
    /// Gets a value indicating whether the open.mp entity counterpart has been destroyed.
    /// </summary>
    protected bool IsOmpEntityDestroyed => _textLabel.TryGetExtension<ComponentExtension>()?.IsOmpEntityDestroyed ?? true;

    /// <summary>Gets or sets the color of this text label.</summary>
    public virtual Color Color
    {
        get
        {
            _textLabel.GetColour(out var colour);
            return colour;
        }
        set => _textLabel.SetColour(value);
    }

    /// <summary>Gets or sets the text of this text label.</summary>
    public virtual string Text
    {
        get => _textLabel.GetText();
        set => _textLabel.SetText(value);
    }

    /// <summary>Gets the draw distance of this text label.</summary>
    public virtual float DrawDistance => _textLabel.GetDrawDistance();
    
    /// <summary>Gets a value indicating whether to test the line of sight.</summary>
    public virtual bool TestLos => _textLabel.GetTestLOS();

    /// <summary>Gets or sets the attached entity (player or vehicle).</summary>
    public virtual Component? AttachedEntity
    {
        get
        {
            var attachmentData = _textLabel.GetAttachmentData();
            
            if (attachmentData.PlayerId != OpenMpConstants.INVALID_PLAYER_ID)
            {
                return _entityProvider.GetPlayer(attachmentData.PlayerId);
            }

            if (attachmentData.VehicleId != OpenMpConstants.INVALID_VEHICLE_ID)
            {
                return _entityProvider.GetVehicle(attachmentData.VehicleId);
            }

            return null;
        }
    }
    
    public virtual void Attach(Player player, Vector3 offset = default)
    {
        _textLabel.AttachToPlayer(player, offset);
    }

    public virtual void Attach(Vehicle vehicle, Vector3 offset = default)
    {
        _textLabel.AttachToVehicle(vehicle, offset);
    }
    
    protected override void OnDestroyComponent()
    {
        if (!IsOmpEntityDestroyed)
        {
            _textLabels.AsPool().Release(Id);
        }
    }

    public override string ToString()
    {
        return $"(Id: {Id}, Text: {Text})";
    }
    
    public static implicit operator ITextLabel(TextLabel textLabel)
    {
        return textLabel._textLabel;
    }
}
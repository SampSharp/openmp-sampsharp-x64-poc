using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>
/// Represents a component which provides an identifier.
/// </summary>
public abstract class IdProvider : Component
{
    private readonly IIDProvider _idProvider;

    protected IdProvider(IIDProvider idProvider)
    {
        _idProvider = idProvider;
    }

    /// <summary>
    /// Gets the identifier of this component.
    /// </summary>
    public virtual int Id => _idProvider.GetID();
}
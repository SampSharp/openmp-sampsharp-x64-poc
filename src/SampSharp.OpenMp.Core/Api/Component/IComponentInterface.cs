namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IComponentInterface"/> interface.
/// </summary>
public interface IComponentInterface
{
    /// <summary>
    /// Gets the identifier of the component type.
    /// </summary>
    static abstract UID ComponentId { get; }
}
namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Marker interface for components. This interface is automatically implemented by the code generator when an open.mp
/// interface struct is marked with the <see cref="OpenMpApiAttribute" /> and marks <see cref="IComponent" /> as a base
/// type.
/// </summary>
public interface IComponentInterface
{
    /// <summary>
    /// Gets the identifier of the component type.
    /// </summary>
    static abstract UID ComponentId { get; }
}
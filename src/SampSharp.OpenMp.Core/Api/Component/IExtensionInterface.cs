namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Marker interface for unmanaged extensions. This interface is automatically implemented by the code generator when an
/// open.mp interface struct is marked with the <see cref="OpenMpApiAttribute" /> and marks <see cref="IExtension" /> as a
/// base type.
/// </summary>
public interface IExtensionInterface
{
    /// <summary>
    /// Gets the identifier of the extension type.
    /// </summary>
    static abstract UID ExtensionId { get; }

    /// <summary>
    /// Gets a value indicating whether this struct pointer has a value.
    /// </summary>
    bool HasValue { get; }
}
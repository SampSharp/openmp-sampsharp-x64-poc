namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IComponent"/> interface.
/// </summary>
[OpenMpApi]
public readonly partial struct IComponent
{
    public partial ComponentType GetComponentType();
    public partial int SupportedVersion();
    public partial string ComponentName();
    public partial SemanticVersion ComponentVersion();
}
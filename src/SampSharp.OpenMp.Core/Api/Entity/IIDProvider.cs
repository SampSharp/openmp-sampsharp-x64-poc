namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IIDProvider" /> interface.
/// </summary>
[OpenMpApi]
public readonly partial struct IIDProvider : IIDProviderInterface
{
    public partial int GetID();
}
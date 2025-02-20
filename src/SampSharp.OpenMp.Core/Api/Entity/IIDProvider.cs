namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IIDProvider : IIDProviderInterface
{
    public partial int GetID();
}
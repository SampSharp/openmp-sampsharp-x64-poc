namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct INetworkComponent
{
    public static UID ComponentId => new(0xea9799fd79cf8442); // ID for RakNetLegacyNetworkComponent

    public partial INetwork GetNetwork();
}
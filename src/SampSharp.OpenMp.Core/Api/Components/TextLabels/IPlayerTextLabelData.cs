using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerTextLabelData"/> interface.
/// </summary>
[OpenMpApi(typeof(IExtension), typeof(IPool<IPlayerTextLabel>))]
public readonly partial struct IPlayerTextLabelData
{
    public static UID ExtensionId => new(0xb9e2bd0dc5148c3c);

    public partial IPlayerTextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, bool los);

    [OpenMpApiOverload("_player")]
    public partial IPlayerTextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, bool los, IPlayer attach);

    [OpenMpApiOverload("_vehicle")]
    public partial IPlayerTextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, bool los, IVehicle attach);
     
    public IPool<IPlayerTextLabel> AsPool()
    {
        return (IPool<IPlayerTextLabel>)this;
    }
}
using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IExtension))]
public readonly partial struct IPlayerTextLabelData
{
    public static UID ExtensionId => new(0xb9e2bd0dc5148c3c);

    public partial IPlayerTextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, bool los);

    [OpenMpApiOverload("_player")]
    public partial IPlayerTextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, bool los, IPlayer attach);

    [OpenMpApiOverload("_vehicle")]
    public partial IPlayerTextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, bool los, IVehicle attach);
}
using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct ITextLabelsComponent
{
    public static UID ComponentId => new(0xa0c57ea80a009742);
    public partial ITextLabel Create(StringView text, Colour colour, Vector3 pos, float drawDist, int vw, bool los);

    [OpenMpApiOverload("_player")]
    public partial ITextLabel Create(StringView text, Colour colour, Vector3 pos, float drawDist, int vw, bool los, IPlayer attach);

    [OpenMpApiOverload("_vehicle")]
    public partial ITextLabel Create(StringView text, Colour colour, Vector3 pos, float drawDist, int vw, bool los, IVehicle attach);
}
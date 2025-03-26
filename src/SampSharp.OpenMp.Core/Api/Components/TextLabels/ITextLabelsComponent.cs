using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="ITextLabelsComponent"/> interface.
/// </summary>
[OpenMpApi(typeof(IPoolComponent<ITextLabel>))]
public readonly partial struct ITextLabelsComponent
{
    public static UID ComponentId => new(0xa0c57ea80a009742);
    public partial ITextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, int vw, bool los);

    [OpenMpApiOverload("_player")]
    public partial ITextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, int vw, bool los, IPlayer attach);

    [OpenMpApiOverload("_vehicle")]
    public partial ITextLabel Create(string text, Colour colour, Vector3 pos, float drawDist, int vw, bool los, IVehicle attach);
}
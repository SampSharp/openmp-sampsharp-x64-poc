using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct ITextDrawsComponent
{
    public static UID ComponentId => new(0x9b5dc2b1d15c992a);

    public partial IEventDispatcher<ITextDrawEventHandler> GetEventDispatcher();
    public partial ITextDraw Create(Vector2 position, string text);

    [OpenMpApiOverload("_model")]
    public partial ITextDraw Create(Vector2 position, int model);
}
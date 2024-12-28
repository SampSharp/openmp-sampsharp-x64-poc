using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IComponent))]
public readonly partial struct ITextDrawsComponent
{
    public static UID ComponentId => new(0x9b5dc2b1d15c992a);

    public partial IEventDispatcher2<ITextDrawEventHandler> GetEventDispatcher();
    public partial ITextDraw Create(Vector2 position, StringView text);

    [OpenMpApiOverload("_model")]
    public partial ITextDraw Create(Vector2 position, int model);
}
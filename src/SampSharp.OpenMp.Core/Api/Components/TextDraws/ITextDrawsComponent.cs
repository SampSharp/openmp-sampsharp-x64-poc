using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="ITextDrawsComponent" /> interface.
/// </summary>
[OpenMpApi(typeof(IPoolComponent<ITextDraw>))]
public readonly partial struct ITextDrawsComponent
{
    public static UID ComponentId => new(0x9b5dc2b1d15c992a);

    public partial IEventDispatcher<ITextDrawEventHandler> GetEventDispatcher();
    public partial ITextDraw Create(Vector2 position, string text);

    [OpenMpApiOverload("_model")]
    public partial ITextDraw Create(Vector2 position, int model);
}
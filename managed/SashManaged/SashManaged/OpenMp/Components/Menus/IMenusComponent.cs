using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct IMenusComponent
{
    public static UID ComponentId => new(0x621e219eb97ee0b2);

    public partial IEventDispatcher<IMenuEventHandler> GetEventDispatcher();

    public partial IMenu Create(string title, Vector2 position, byte columns, float col1Width, float col2Width);
}
using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IMenusComponent" /> interface.
/// </summary>
[OpenMpApi(typeof(IPoolComponent<IMenu>))]
public readonly partial struct IMenusComponent
{
    /// <inheritdoc />
    public static UID ComponentId => new(0x621e219eb97ee0b2);

    public partial IEventDispatcher<IMenuEventHandler> GetEventDispatcher();

    public partial IMenu Create(string title, Vector2 position, byte columns, float col1Width, float col2Width);
}
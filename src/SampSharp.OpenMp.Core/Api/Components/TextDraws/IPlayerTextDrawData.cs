using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerTextDrawData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension), typeof(IPool<IPlayerTextDraw>))]
public readonly partial struct IPlayerTextDrawData
{
    /// <inheritdoc />
    public static UID ExtensionId => new(0xbf08495682312400);

    public partial void BeginSelection(Colour highlight);
    public partial bool IsSelecting();
    public partial void EndSelection();
    public partial IPlayerTextDraw Create(Vector2 position, string text);

    [OpenMpApiOverload("_model")]
    public partial IPlayerTextDraw Create(Vector2 position, int model);
    
    public IPool<IPlayerTextDraw> AsPool()
    {
        return (IPool<IPlayerTextDraw>)this;
    }
}
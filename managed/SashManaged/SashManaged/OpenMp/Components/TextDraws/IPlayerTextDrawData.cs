using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerTextDrawData
{
    public static UID ExtensionId => new(0xbf08495682312400);

    public partial void BeginSelection(Colour highlight);
    public partial bool IsSelecting();
    public partial void EndSelection();
    public partial IPlayerTextDraw Create(Vector2 position, StringView text);

    [OpenMpApiOverload("_model")]
    public partial IPlayerTextDraw Create(Vector2 position, int model);
}
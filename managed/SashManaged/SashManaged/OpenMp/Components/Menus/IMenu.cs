using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IExtensible), typeof(IIDProvider))]
public readonly partial struct IMenu
{
    public partial void SetColumnHeader(StringView header, byte column);
    public partial int AddCell(StringView itemText, byte column);
    public partial void DisableRow(byte row);
    public partial bool IsRowEnabled(byte row);
    public partial void Disable();
    public partial bool IsEnabled();
    public partial ref Vector2 GetPosition();
    public partial int GetRowCount(byte column);
    public partial int GetColumnCount();
    public partial Vector2 GetColumnWidths();
    public partial StringView GetColumnHeader(byte column);
    public partial StringView GetCell(byte column, byte row);
    public partial void InitForPlayer(IPlayer player);
    public partial void ShowForPlayer(IPlayer player);
    public partial void HideForPlayer(IPlayer player);
}
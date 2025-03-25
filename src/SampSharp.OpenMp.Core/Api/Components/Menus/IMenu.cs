using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtensible), typeof(IIDProvider))]
public readonly partial struct IMenu
{
    public partial void SetColumnHeader(string header, byte column);
    public partial int AddCell(string itemText, byte column);
    public partial void DisableRow(byte row);
    public partial bool IsRowEnabled(byte row);
    public partial void Disable();
    public partial bool IsEnabled();
    public partial ref Vector2 GetPosition();
    public partial int GetRowCount(byte column);
    public partial int GetColumnCount();
    private partial void GetColumnWidths(out Vector2 widths);

    public Vector2 GetColumnWidths()
    {
        GetColumnWidths(out var result);
        return result;
    }

    public partial string? GetColumnHeader(byte column);
    public partial string? GetCell(byte column, byte row);
    public partial void InitForPlayer(IPlayer player);
    public partial void ShowForPlayer(IPlayer player);
    public partial void HideForPlayer(IPlayer player);
}
namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="ITextDraw"/> interface.
/// </summary>
[OpenMpApi(typeof(ITextDrawBase))]
public readonly partial struct ITextDraw
{
    public partial void ShowForPlayer(IPlayer player);
    public partial void HideForPlayer(IPlayer player);
    public partial bool IsShownForPlayer(IPlayer player);
    public partial void SetTextForPlayer(IPlayer player, string text);
}
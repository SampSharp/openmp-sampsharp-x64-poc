namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerTextDraw" /> interface.
/// </summary>
[OpenMpApi(typeof(ITextDrawBase))]
public readonly partial struct IPlayerTextDraw
{
    public partial void Show();
    public partial void Hide();
    public partial bool IsShown();
}
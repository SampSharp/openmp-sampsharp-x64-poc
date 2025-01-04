namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(ITextDrawBase))]
public readonly partial struct IPlayerTextDraw
{
    public partial void Show();
    public partial void Hide();
    public partial bool IsShown();
}
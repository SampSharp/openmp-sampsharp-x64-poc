namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerDialogData
{
    public static UID ExtensionId => new(0xbc03376aa3591a11);

    public partial void Hide(IPlayer player);
    public partial void Show(IPlayer player, int id, DialogStyle style, string title, string body, string button1, string button2);
    public partial void Get(ref int id, ref DialogStyle style, ref string title, ref string body, ref string button1, ref string button2);
    public partial int GetActiveID();
}
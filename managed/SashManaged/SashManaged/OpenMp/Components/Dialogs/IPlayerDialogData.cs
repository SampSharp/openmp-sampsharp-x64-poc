namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IExtension))]
public readonly partial struct IPlayerDialogData
{
    public static UID ExtensionId => new(0xbc03376aa3591a11);

    public partial void Hide(IPlayer player);
    public partial void Show(IPlayer player, int id, DialogStyle style, StringView title, StringView body, StringView button1, StringView button2);
    public partial void Get(ref int id, ref DialogStyle style, ref StringView title, ref StringView body, ref StringView button1, ref StringView button2);
    public partial int GetActiveID();
}
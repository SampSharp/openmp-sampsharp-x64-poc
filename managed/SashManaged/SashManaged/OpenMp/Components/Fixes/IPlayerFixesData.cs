namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerFixesData
{
    public static UID ExtensionId => new(0x672d5d6fbb094ef7);

    public partial bool SendGameText(string message, Milliseconds time, int style);
    public partial bool HideGameText(int style);
    public partial bool HasGameText(int style);
    public partial bool GetGameText(int style, ref string message, ref Milliseconds time, ref Milliseconds remaining);
    public partial void ApplyAnimation(IPlayer player, IActor actor, ref AnimationData animation);
}
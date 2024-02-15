namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct IComponent
{
    public partial int SupportedVersion();
    public partial StringView ComponentName();
}
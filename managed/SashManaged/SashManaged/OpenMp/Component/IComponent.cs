namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct IComponent
{
    public partial ComponentType GetComponentType();
    public partial int SupportedVersion();
    public partial StringView ComponentName();
    public partial SemanticVersion ComponentVersion();
}
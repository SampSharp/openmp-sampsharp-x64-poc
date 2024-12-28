namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct IComponent
{
    public partial ComponentType GetComponentType();
    public partial int SupportedVersion();
    public partial string ComponentName();
    public partial SemanticVersion ComponentVersion();
}
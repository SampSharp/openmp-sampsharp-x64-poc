namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IExtensible))]
public readonly partial struct IPlayerConsoleData
{
    public partial bool HasConsoleAccess();
    public partial void SetConsoleAccessibility(bool set);
}
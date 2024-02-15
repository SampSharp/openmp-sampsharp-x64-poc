namespace SashManaged;

[AttributeUsage(AttributeTargets.Method)]
public class OpenMpApiOverloadAttribute(string overload) : Attribute
{
    public string Overload { get; } = overload;
}
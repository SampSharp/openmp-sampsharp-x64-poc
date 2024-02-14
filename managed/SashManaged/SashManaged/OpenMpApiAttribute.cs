namespace SashManaged;

[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpApiAttribute(params Type[] implements) : Attribute
{
    public Type[] Implements { get; } = implements;
}

[AttributeUsage(AttributeTargets.Method)]
public class OpenMpApiOverloadAttribute(string overload) : Attribute
{
    public string Overload { get; } = overload;
}
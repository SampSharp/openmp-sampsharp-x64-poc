namespace SashManaged;

[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpApiAttribute(params Type[] implements) : Attribute
{
    public Type[] Implements { get; } = implements;
}
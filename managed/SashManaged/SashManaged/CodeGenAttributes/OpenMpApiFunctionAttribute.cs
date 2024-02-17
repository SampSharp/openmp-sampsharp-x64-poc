namespace SashManaged;

[AttributeUsage(AttributeTargets.Method)]
public class OpenMpApiFunctionAttribute(string functionName) : Attribute
{
    public string FunctionName { get; } = functionName;
}
namespace SashManaged;

[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpHybridStringGeneratorAttribute(int Size) : Attribute
{
    public int Size { get; } = Size;
}
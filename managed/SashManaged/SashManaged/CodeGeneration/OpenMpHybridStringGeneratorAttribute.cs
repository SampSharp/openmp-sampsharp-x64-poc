namespace SashManaged;

[Obsolete("Should be replaced with custom marshaller for the specific occasion.")]
[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpHybridStringGeneratorAttribute(int Size) : Attribute
{
    public int Size { get; } = Size;
}
namespace SashManaged;

[Obsolete("Should be replaced with custom marshaller for the specific occasion.")]
[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpHybridStringGeneratorAttribute(int size) : Attribute
{
    public int Size { get; } = size;
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public class NumberedTypeGeneratorAttribute(string fieldName, int value) : Attribute
{
    public string FieldName { get; } = fieldName;
    public int Value { get; } = value;
}
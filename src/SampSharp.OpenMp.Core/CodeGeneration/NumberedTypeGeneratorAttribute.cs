namespace SampSharp.OpenMp.Core;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public class NumberedTypeGeneratorAttribute(string fieldName, int value) : Attribute
{
    public string FieldName { get; } = fieldName;
    public int Value { get; } = value;
}
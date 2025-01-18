namespace SampSharp.SourceGenerator.Marshalling.V2;

public interface ICustomMarshalGeneratorFactory
{
    IMarshalShapeGenerator Create(MarshallerShape shape);
}
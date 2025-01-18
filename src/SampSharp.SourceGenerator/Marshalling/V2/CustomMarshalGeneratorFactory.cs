using SampSharp.SourceGenerator.Marshalling.V2.ShapeGenerators;

namespace SampSharp.SourceGenerator.Marshalling.V2;

public static class CustomMarshalGeneratorFactory
{
    public static IMarshalShapeGenerator Create(MarshallerShape shape, bool? stateful)
    {
        return !stateful.HasValue || shape == MarshallerShape.None // fast path
            ? EmptyMarshalShapeGenerator.Instance
            : GetFactory(stateful.Value).Create(shape);
    }

    private static ICustomMarshalGeneratorFactory GetFactory(bool stateful)
    {
        return stateful
            ? StatefulCustomMarshalGeneratorFactory.Instance
            : StatelessCustomMarshalGeneratorFactory.Instance;
    }
}
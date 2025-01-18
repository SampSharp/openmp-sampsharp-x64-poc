using Microsoft.CodeAnalysis;

namespace SampSharp.SourceGenerator.Marshalling;

public class IdentifierStubContextFactory
{
    private readonly CustomMarshallerTypeDetector _customMarshallerTypeDetector;

    public IdentifierStubContextFactory(WellKnownMarshallerTypes wellKnownMarshallerTypes)
    {
        _customMarshallerTypeDetector = new CustomMarshallerTypeDetector(wellKnownMarshallerTypes);
    }

    public IdentifierStubContext Create(IParameterSymbol parameter, MarshalDirection marshalDirection)
    {
        var customMarshaller = _customMarshallerTypeDetector.GetCustomMarshaller(parameter, marshalDirection);
        var shape = customMarshaller == null ? MarshallerShape.None : ShapeDetector.GetShapeOfMarshaller(customMarshaller);

        var generator = CustomMarshalGeneratorFactory.Create(shape, customMarshaller?.IsStateful);

        var mem = customMarshaller == null ? null : MarshalInspector.GetMembers(customMarshaller);
        return new IdentifierStubContext(parameter, marshalDirection, parameter.Type, customMarshaller, mem, shape, generator);
    }

    public IdentifierStubContext Create(IMethodSymbol method, MarshalDirection marshalDirection)
    {
        var customMarshaller = _customMarshallerTypeDetector.GetCustomMarshaller(method, marshalDirection);
        var shape = customMarshaller == null ? MarshallerShape.None : ShapeDetector.GetShapeOfMarshaller(customMarshaller);

        var generator = CustomMarshalGeneratorFactory.Create(shape, customMarshaller?.IsStateful);

        var mem = customMarshaller == null ? null : MarshalInspector.GetMembers(customMarshaller);
        return new IdentifierStubContext(null, marshalDirection, method.ReturnType, customMarshaller, mem, shape, generator);
    }
}
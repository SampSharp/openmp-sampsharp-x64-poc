using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace SampSharp.SourceGenerator.Marshalling.V2;

public class IdentifierStubContextFactory
{
    private readonly CustomMarshallerTypeFinder _customMarshallerTypeFinder;

    public IdentifierStubContextFactory(WellKnownMarshallerTypes wellKnownMarshallerTypes)
    {
        _customMarshallerTypeFinder = new CustomMarshallerTypeFinder(wellKnownMarshallerTypes);
    }

    public IdentifierStubContext Create(IParameterSymbol parameter, MarshalDirection marshalDirection)
    {
        var customMarshaller = _customMarshallerTypeFinder.GetCustomMarshaller(parameter, marshalDirection);
        var shape = customMarshaller == null ? MarshallerShape.None : ShapeTool.GetShapeOfMarshaller(customMarshaller);

        var generator = CustomMarshalGeneratorFactory.Create(shape, customMarshaller?.IsStateful);
                    
        var mem = customMarshaller == null ? null : MarshalInspector.GetMembers(customMarshaller);    
        return new IdentifierStubContext(parameter, marshalDirection, parameter.Type, customMarshaller,  mem, shape, generator);
    }
    
    public IdentifierStubContext Create(IMethodSymbol method, MarshalDirection marshalDirection)
    {
        var customMarshaller = _customMarshallerTypeFinder.GetCustomMarshaller(method, marshalDirection);
        var shape = customMarshaller == null ? MarshallerShape.None : ShapeTool.GetShapeOfMarshaller(customMarshaller);

        var generator = CustomMarshalGeneratorFactory.Create(shape, customMarshaller?.IsStateful);

        var mem = customMarshaller == null ? null : MarshalInspector.GetMembers(customMarshaller);
        return new IdentifierStubContext(null, marshalDirection, method.ReturnType, customMarshaller, mem, shape, generator);
    }
}
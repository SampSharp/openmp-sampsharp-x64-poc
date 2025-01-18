using Microsoft.CodeAnalysis;

namespace SampSharp.SourceGenerator.Marshalling;

public class IdentifierStubContextFactory
{    
    //
    // stages:
    // during context building:
    // 1. Decide which marshaller implementation to use based on entry point of the specified custom marshaller ( CustomMarshallerTypeFinder )
    // 2. Deduce shape based on the implementation ( ShapeTool )
    // 3. Activate ShapeGenerator based on shape (CustomMarshalGeneratorFactory.Create)
    // during generation:
    // 4. Generate marshaling code and combine with invocation code
    //

    private readonly CustomMarshallerTypeDetector _customMarshallerTypeDetector;

    public IdentifierStubContextFactory(WellKnownMarshallerTypes wellKnownMarshallerTypes)
    {
        _customMarshallerTypeDetector = new CustomMarshallerTypeDetector(wellKnownMarshallerTypes);
    }

    public IdentifierStubContext Create(IParameterSymbol parameter, MarshalDirection marshalDirection)
    {
        var customMarshaller = _customMarshallerTypeDetector.GetCustomMarshaller(parameter, marshalDirection);
        var (shape, nativeType) = customMarshaller == null 
            ? (MarshallerShape.None, new ManagedType(parameter.Type))
            : ShapeDetector.GetShapeOfMarshaller(customMarshaller);

        var generator = CustomMarshalGeneratorFactory.Create(shape, customMarshaller?.IsStateful);

        return new IdentifierStubContext(marshalDirection, new ManagedType(parameter.Type), customMarshaller?.MarshallerType, nativeType, shape, generator, parameter.RefKind, parameter.Name);
    }

    public IdentifierStubContext Create(IMethodSymbol method, MarshalDirection marshalDirection)
    {
        var customMarshaller = _customMarshallerTypeDetector.GetCustomMarshaller(method, marshalDirection);
        var (shape, nativeType) = customMarshaller == null 
            ? (MarshallerShape.None, new ManagedType(method.ReturnType)) 
            : ShapeDetector.GetShapeOfMarshaller(customMarshaller);

        var generator = CustomMarshalGeneratorFactory.Create(shape, customMarshaller?.IsStateful);

        return new IdentifierStubContext(marshalDirection, new ManagedType(method.ReturnType), customMarshaller?.MarshallerType, nativeType, shape, generator, RefKind.Out, MarshallerConstants.LocalReturnValue);
    }
}
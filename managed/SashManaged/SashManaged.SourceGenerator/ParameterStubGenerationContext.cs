using Microsoft.CodeAnalysis;
using SashManaged.SourceGenerator.Marshalling;

namespace SashManaged.SourceGenerator;

public record struct ParameterStubGenerationContext(IParameterSymbol Symbol, IMarshallerShape MarshallerShape);
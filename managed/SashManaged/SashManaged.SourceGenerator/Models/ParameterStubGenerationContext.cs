using Microsoft.CodeAnalysis;
using SashManaged.SourceGenerator.Marshalling.Shapes;

namespace SashManaged.SourceGenerator.Models;

public record struct ParameterStubGenerationContext(IParameterSymbol Symbol, IMarshallerShape? MarshallerShape);
using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Marshalling.Shapes;

namespace SampSharp.SourceGenerator.Models;

public record struct ParameterStubGenerationContext(IParameterSymbol Symbol, IMarshallerShape? MarshallerShape);
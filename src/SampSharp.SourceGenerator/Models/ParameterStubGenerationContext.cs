using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.Marshalling.V2;

namespace SampSharp.SourceGenerator.Models;

public record struct ParameterStubGenerationContext(IParameterSymbol Symbol, IMarshallerShape? MarshallerShape, IdentifierStubContext V2Ctx);
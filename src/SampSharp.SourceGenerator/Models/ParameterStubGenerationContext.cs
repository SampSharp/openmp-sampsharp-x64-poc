using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Marshalling;

namespace SampSharp.SourceGenerator.Models;

public record struct ParameterStubGenerationContext(IParameterSymbol Symbol, IdentifierStubContext V2Ctx);
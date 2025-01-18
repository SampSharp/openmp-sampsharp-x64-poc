using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Marshalling;

namespace SampSharp.SourceGenerator.Models;

public record MarshallingStubGenerationContext(
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters,
    IdentifierStubContext ReturnV2Ctx,
    bool RequiresMarshalling)
{
    public bool ReturnsByRef => Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly;
}
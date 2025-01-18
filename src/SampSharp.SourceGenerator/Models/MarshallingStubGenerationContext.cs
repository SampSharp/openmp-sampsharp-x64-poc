using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Marshalling;

namespace SampSharp.SourceGenerator.Models;

public record MarshallingStubGenerationContext(
    IMethodSymbol Symbol,
    IdentifierStubContext[] Parameters,
    IdentifierStubContext ReturnValue,
    bool RequiresMarshalling)
{
    public bool ReturnsByRef => Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly;
}
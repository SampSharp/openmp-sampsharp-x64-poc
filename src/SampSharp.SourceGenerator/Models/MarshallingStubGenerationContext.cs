using Microsoft.CodeAnalysis;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.Marshalling.V2;

namespace SampSharp.SourceGenerator.Models;

public record MarshallingStubGenerationContext(
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters,
    IMarshallerShape? ReturnMarshallerShape,
    IdentifierStubContext ReturnV2Ctx,
    bool RequiresMarshalling)
{
    public bool ReturnsByRef => Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly;
}
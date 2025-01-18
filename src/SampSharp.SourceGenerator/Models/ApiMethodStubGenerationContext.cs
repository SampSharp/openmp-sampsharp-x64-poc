using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling;

namespace SampSharp.SourceGenerator.Models;

public record ApiMethodStubGenerationContext(
    MethodDeclarationSyntax Declaration,
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters,
    IdentifierStubContext ReturnV2Ctx,
    bool RequiresMarshalling,
    string Library,
    string NativeTypeName) : MarshallingStubGenerationContext(Symbol, Parameters, ReturnV2Ctx, RequiresMarshalling);
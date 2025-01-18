using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.Marshalling.V2;

namespace SampSharp.SourceGenerator.Models;

public record ApiMethodStubGenerationContext(
    MethodDeclarationSyntax Declaration,
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters,
    IdentifierStubContext ReturnV2Ctx,
    IMarshallerShape? ReturnMarshallerShape,
    bool RequiresMarshalling,
    string Library,
    string NativeTypeName) : MarshallingStubGenerationContext(Symbol, Parameters, ReturnMarshallerShape, ReturnV2Ctx, RequiresMarshalling);
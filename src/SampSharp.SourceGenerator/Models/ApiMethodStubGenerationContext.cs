using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling.Shapes;

namespace SampSharp.SourceGenerator.Models;

public record ApiMethodStubGenerationContext(
    MethodDeclarationSyntax Declaration,
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters,
    IMarshallerShape? ReturnMarshallerShape,
    bool RequiresMarshalling,
    string Library,
    string NativeTypeName) : MarshallingStubGenerationContext(Symbol, Parameters, ReturnMarshallerShape, RequiresMarshalling);
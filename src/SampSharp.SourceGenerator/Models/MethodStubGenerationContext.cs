using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling.Shapes;

namespace SampSharp.SourceGenerator.Models;

public record MethodStubGenerationContext(
    MethodDeclarationSyntax Declaration,
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters,
    IMarshallerShape? ReturnMarshallerShape,
    bool RequiresMarshalling,
    string Library,
    string NativeTypeName)
{
    public bool ReturnsByRef => Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly;
}
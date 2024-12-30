using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Marshalling.Shapes;

namespace SashManaged.SourceGenerator.Models;

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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Marshalling;

namespace SashManaged.SourceGenerator;

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
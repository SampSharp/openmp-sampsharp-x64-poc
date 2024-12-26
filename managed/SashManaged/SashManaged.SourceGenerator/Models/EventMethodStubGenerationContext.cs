using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Marshalling;

namespace SashManaged.SourceGenerator;

public record EventMethodStubGenerationContext(
    MethodDeclarationSyntax Declaration, 
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters, 
    IMarshallerShape? ReturnMarshallerShape, 
    bool RequiresMarshalling);
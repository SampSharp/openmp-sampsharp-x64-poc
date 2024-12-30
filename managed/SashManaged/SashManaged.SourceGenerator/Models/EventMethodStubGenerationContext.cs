using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Marshalling.Shapes;

namespace SashManaged.SourceGenerator.Models;

public record EventMethodStubGenerationContext(
    MethodDeclarationSyntax Declaration, 
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters, 
    IMarshallerShape? ReturnMarshallerShape, 
    bool RequiresMarshalling);
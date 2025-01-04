using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling.Shapes;

namespace SampSharp.SourceGenerator.Models;

public record EventMethodStubGenerationContext(
    MethodDeclarationSyntax Declaration, 
    IMethodSymbol Symbol,
    ParameterStubGenerationContext[] Parameters, 
    IMarshallerShape? ReturnMarshallerShape, 
    bool RequiresMarshalling);
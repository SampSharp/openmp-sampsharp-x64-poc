using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SampSharp.SourceGenerator.Models;

public record EventInterfaceStubGenerationContext(
    INamedTypeSymbol Symbol,
    InterfaceDeclarationSyntax Syntax,
    MarshallingStubGenerationContext[] Methods,
    string Library,
    string NativeTypeName);
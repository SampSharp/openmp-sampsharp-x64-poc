using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator;

public record StructStubGenerationContext(
    ISymbol Symbol,
    StructDeclarationSyntax Node,
    MethodStubGenerationContext[] Methods,
    ITypeSymbol[] ImplementingTypes);
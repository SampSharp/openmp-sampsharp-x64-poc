using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Models;

public record StructStubGenerationContext(
    ISymbol Symbol,
    StructDeclarationSyntax Syntax,
    MethodStubGenerationContext[] Methods,
    ITypeSymbol[] ImplementingTypes,
    bool IsExtension,
    bool IsComponent);
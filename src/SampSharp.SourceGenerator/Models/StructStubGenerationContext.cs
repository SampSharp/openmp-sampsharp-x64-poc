using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SampSharp.SourceGenerator.Models;

public record StructStubGenerationContext(
    ISymbol Symbol,
    StructDeclarationSyntax Syntax,
    ApiMethodStubGenerationContext[] Methods,
    ImplementingType[] ImplementingTypes,
    bool IsExtension,
    bool IsComponent,
    string Library);

public readonly record struct ImplementingType(ITypeSymbol Type, ITypeSymbol[] CastPath);
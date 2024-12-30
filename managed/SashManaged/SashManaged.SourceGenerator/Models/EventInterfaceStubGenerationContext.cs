using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Models;

public record EventInterfaceStubGenerationContext(
    ISymbol Symbol,
    InterfaceDeclarationSyntax Syntax,
    EventMethodStubGenerationContext[] Methods,
    string Library,
    string NativeTypeName);
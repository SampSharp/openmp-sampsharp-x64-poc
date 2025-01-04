using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SampSharp.SourceGenerator.Helpers;

public static class SymbolExtensions
{
    public static bool IsPartial(this MemberDeclarationSyntax syntax)
    {
        return syntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
    }

    public static bool HasModifier(this MemberDeclarationSyntax syntax, SyntaxKind modifier)
    {
        return syntax.Modifiers.Any(m => m.IsKind(modifier));
    }

    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes().GetAttributes(attributeName);
    }

    public static AttributeData? GetAttribute(this ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes(attributeName)
            .FirstOrDefault();
    }

    public static AttributeData? GetReturnTypeAttribute(this IMethodSymbol symbol, string attributeName)
    {
        return symbol.GetReturnTypeAttributes(attributeName)
            .FirstOrDefault();
    }

    public static bool IsSame(this ITypeSymbol symbol, string typeFQN)
    {
        return string.Equals(symbol.ToDisplayString(FullyQualifiedFormatWithoutGlobal), typeFQN, StringComparison.Ordinal);
    }

    public static bool IsSame(this ISymbol symbol, ISymbol other)
    {
        return SymbolEqualityComparer.Default.Equals(symbol, other);
    }

    private static IEnumerable<AttributeData> GetReturnTypeAttributes(this IMethodSymbol symbol, string attributeName)
    {
        return symbol.GetReturnTypeAttributes()
            .GetAttributes(attributeName);
    }

    private static IEnumerable<AttributeData> GetAttributes(this ImmutableArray<AttributeData> attribute, string attributeName)
    {
        return attribute
            .Where(x =>
                string.Equals(
                    x.AttributeClass?.ToDisplayString(FullyQualifiedFormatWithoutGlobal),
                    attributeName,
                    StringComparison.Ordinal
                )
            );
    }

    private static readonly SymbolDisplayFormat FullyQualifiedFormatWithoutGlobal =
        SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining);
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator;

public static class SymbolExtensions
{
        
    public static bool HasAttribute(this ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes().HasAttribute(attributeName);
    }
    
    public static bool IsPartial(this MemberDeclarationSyntax syntax)
    {
        return syntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
    }

    public static bool HasModifier(this MemberDeclarationSyntax syntax, SyntaxKind modifier)
    {
        return syntax.Modifiers.Any(m => m.IsKind(modifier));
    }

    public static bool HasAttribute(this ImmutableArray<AttributeData> attributes, string attributeName)
    {
        return attributes.Any(x =>
                string.Equals(
                    x.AttributeClass?.ToDisplayString(_fullyQualifiedFormatWithoutGlobal),
                    attributeName,
                    StringComparison.Ordinal
                )
            );
    } 
    
    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes().GetAttributes(attributeName);
    }

    public static AttributeData GetAttribute(this ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes(attributeName)
            .FirstOrDefault();
    }
    
    public static IEnumerable<AttributeData> GetReturnTypeAttributes(this IMethodSymbol symbol, string attributeName)
    {
        return symbol.GetReturnTypeAttributes()
            .GetAttributes(attributeName);
    }

    public static AttributeData GetReturnTypeAttribute(this IMethodSymbol symbol, string attributeName)
    {
        return symbol.GetReturnTypeAttributes(attributeName)
            .FirstOrDefault();
    }

    public static IEnumerable<AttributeData> GetAttributes(this ImmutableArray<AttributeData> attribute, string attributeName)
    {
        return attribute
            .Where(x =>
                string.Equals(
                    x.AttributeClass?.ToDisplayString(_fullyQualifiedFormatWithoutGlobal),
                    attributeName,
                    StringComparison.Ordinal
                )
            );
    }
    
    private static readonly SymbolDisplayFormat _fullyQualifiedFormatWithoutGlobal =
        SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining);
}
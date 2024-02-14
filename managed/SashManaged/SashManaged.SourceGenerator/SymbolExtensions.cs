using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator;

public static class SymbolExtensions
{
        
    public static bool HasAttribute(this ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes().HasAttribute(attributeName);
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
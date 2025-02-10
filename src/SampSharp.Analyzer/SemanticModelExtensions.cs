using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SampSharp.Analyzer;

public static class SemanticModelExtensions
{
    
    public static bool HasAttribute(this SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, INamedTypeSymbol attributeType)
    {
        foreach (var attributeList in classDeclaration.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                
                var symbol = semanticModel.GetTypeInfo(attribute).Type;
                if (symbol == null)
                {
                    continue;
                }

                if (SymbolEqualityComparer.Default.Equals(symbol, attributeType))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool IsBaseType(this SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, INamedTypeSymbol baseType)
    {
        return classDeclaration.BaseList?.Types
            .Select(baseTypeSyntax => semanticModel.GetTypeInfo(baseTypeSyntax.Type).Type)
            .Any(baseTypeSymbol => SymbolEqualityComparer.Default.Equals(baseTypeSymbol, baseType)) ?? false;
    }
}
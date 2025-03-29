using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator;

public class FullyQualifiedTypeRewriter : CSharpSyntaxRewriter
{
    private readonly SemanticModel _semanticModel;

    public FullyQualifiedTypeRewriter(SemanticModel semanticModel)
    {
        _semanticModel = semanticModel;
    }

    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        var symbolInfo = _semanticModel.GetSymbolInfo(node);
        var symbol = symbolInfo.Symbol;

        if (symbol != null)
        {
            return IdentifierName(ToFQN(symbol));
        }

        return base.VisitIdentifierName(node);
    }

    public override SyntaxNode? VisitQualifiedName(QualifiedNameSyntax node)
    {
        var symbolInfo = _semanticModel.GetSymbolInfo(node);
        var symbol = symbolInfo.Symbol;

        if (symbol != null)
        {
            return IdentifierName(ToFQN(symbol));
        }

        return base.VisitQualifiedName(node);
    }

    public override SyntaxNode? VisitGenericName(GenericNameSyntax node)
    {
        var symbolInfo = _semanticModel.GetSymbolInfo(node);
        var symbol = symbolInfo.Symbol;

        if (symbol != null)
        {
            var fullyQualifiedName = ToFQN(symbol);
            var identifier = fullyQualifiedName.Split('<')[0]; // Get the identifier part
            var typeArguments = node.TypeArgumentList.Arguments.Select(arg => (TypeSyntax)Visit(arg)).ToArray();
            return GenericName(identifier).WithTypeArgumentList(TypeArgumentList(SeparatedList(typeArguments)));
        }

        return base.VisitGenericName(node);
    }

    private static string ToFQN(ISymbol symbol)
    {
        var fqn = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        if (symbol is ITypeParameterSymbol)
        {
            return fqn;
        }
      
        if (!fqn.StartsWith("global::"))
        {
            return $"global::{fqn}";
        }

        return fqn;
    }
}

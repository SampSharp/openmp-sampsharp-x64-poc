using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.SyntaxFactories;

public static class TypeSyntaxFactory
{
    public static IdentifierNameSyntax IdentifierNameGlobal(ITypeSymbol symbol)
    {
        return IdentifierName(TypeStringGlobal(symbol));
    }

    public static IdentifierNameSyntax IdentifierNameGlobal(string typeFQN)
    {
        return IdentifierName(TypeStringGlobal(typeFQN));
    }

    public static SyntaxToken IdentifierGlobal(ITypeSymbol symbol)
    {
        return Identifier(TypeStringGlobal(symbol));
    }

    public static SyntaxToken IdentifierGlobal(string typeFQN)
    {
        return Identifier(TypeStringGlobal(typeFQN));
    }

    public static TypeSyntax TypeNameGlobal(string typeFQN)
    {
        return ParseTypeName(TypeStringGlobal(typeFQN));
    }

    public static TypeSyntax TypeNameGlobal(ITypeSymbol symbol)
    {
        return ParseTypeName(
            symbol.SpecialType == SpecialType.None
                ? TypeStringGlobal(symbol)
                : symbol.ToDisplayString());
    }

    public static TypeSyntax TypeNameGlobal(IMethodSymbol returnTypeOfMethod)
    {
        var result = TypeNameGlobal(returnTypeOfMethod.ReturnType);

        if (returnTypeOfMethod.ReturnsByRef || returnTypeOfMethod.ReturnsByRefReadonly)
        {
            result = returnTypeOfMethod.ReturnsByRefReadonly
                ? RefType(result).WithReadOnlyKeyword(Token(SyntaxKind.ReadOnlyKeyword))
                : RefType(result);
        }

        return result;
    }

    public static string TypeStringGlobal(string typeFQN)
    {
        return $"global::{typeFQN}";
    }

    public static string TypeStringGlobal(ITypeSymbol symbol)
    {
        return TypeStringGlobal(symbol.ToDisplayString());
    }

}
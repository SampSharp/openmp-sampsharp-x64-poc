using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.SyntaxFactories;

/// <summary>
/// Creates type syntax nodes.
/// </summary>
public static class TypeSyntaxFactory
{
    public static TypeSyntax IntPtrType { get; } = ParseTypeName("nint");
    public static TypeSyntax ObjectType { get; } = PredefinedType(Token(SyntaxKind.ObjectKeyword));

    public static IdentifierNameSyntax IdentifierNameGlobal(string typeFQN)
    {
        return IdentifierName(ToGlobalTypeString(typeFQN));
    }

    public static SyntaxToken IdentifierGlobal(string typeFQN)
    {
        return Identifier(ToGlobalTypeString(typeFQN));
    }

    public static TypeSyntax TypeNameGlobal(string typeFQN)
    {
        return ParseTypeName(ToGlobalTypeString(typeFQN));
    }

    public static TypeSyntax TypeNameGlobal(ITypeSymbol symbol)
    {
        if (symbol.TypeKind == TypeKind.TypeParameter)
        {
            return ParseTypeName(symbol.Name);
        }

        return ParseTypeName(
            symbol.SpecialType == SpecialType.None
                ? ToGlobalTypeString(symbol)
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
    
    public static TypeSyntax GenericType(string typeFQN, TypeSyntax typeArgument)
    {
        return GenericName(
                IdentifierGlobal(typeFQN))
            .WithTypeArgumentList(
                TypeArgumentList(
                    SingletonSeparatedList(typeArgument)));
    }

    public static string ToGlobalTypeString(string typeFQN)
    {
        return $"global::{typeFQN}";
    }

    public static string ToGlobalTypeString(ITypeSymbol symbol)
    {
        
        return symbol.SpecialType == SpecialType.None && symbol.TypeKind != TypeKind.Pointer
            ? ToGlobalTypeString(symbol.ToDisplayString()) 
            : symbol.ToDisplayString();
    }
}
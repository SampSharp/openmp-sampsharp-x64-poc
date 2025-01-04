using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.SyntaxFactories;

/// <summary>
/// Creates common statement syntax nodes.
/// </summary>
public static class StatementFactory
{
    public static LocalDeclarationStatementSyntax CreateLocalDeclarationWithDefaultValue(TypeSyntax type, string identifier)
    {
        return LocalDeclarationStatement(
            VariableDeclaration(type,
                SingletonSeparatedList(
                    VariableDeclarator(Identifier(identifier))
                        .WithInitializer(
                            EqualsValueClause(
                                LiteralExpression(SyntaxKind.DefaultLiteralExpression, Token(SyntaxKind.DefaultKeyword)))))));
    }
}
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators.ApiStructs;

public static class EqualityMembersGenerator
{
    public static IEnumerable<MemberDeclarationSyntax> GenerateEqualityMembers(StructStubGenerationContext ctx)
    {
        // public override bool Equals(object obj)
        yield return MethodDeclaration(
            PredefinedType(Token(SyntaxKind.BoolKeyword)),
            Identifier("Equals"))
        .WithModifiers(
            TokenList(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.OverrideKeyword)
            ))
        .WithParameterList(
            ParameterList(
                SingletonSeparatedList(
                    Parameter(
                        Identifier("obj"))
                    .WithType(
                            NullableType(ObjectType)))))
        .WithBody(
            Block(
                IfStatement(
                    BinaryExpression(
                        SyntaxKind.LogicalAndExpression,
                        IsPatternExpression(
                            IdentifierName("obj"),
                            ConstantPattern(
                                LiteralExpression(SyntaxKind.NullLiteralExpression))),
                        BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            IdentifierName("Handle"),
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IntPtrType,
                                IdentifierName("Zero")))),
                    Block(
                        SingletonList<StatementSyntax>(
                            ReturnStatement(
                                LiteralExpression(SyntaxKind.TrueLiteralExpression))))),
                ReturnStatement(
                    BinaryExpression(
                        SyntaxKind.LogicalAndExpression,
                        IsPatternExpression(
                            IdentifierName("obj"),
                            DeclarationPattern(
                                IdentifierName(ctx.Symbol.Name),
                                SingleVariableDesignation(
                                    Identifier("other")))),
                        InvocationExpression(
                            IdentifierName("Equals"))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(IdentifierName("other")))))))));

        // public override int GetHashCode()
        yield return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.IntKeyword)),
                Identifier("GetHashCode"))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.OverrideKeyword)
                ))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ReturnStatement(
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("_handle"),
                                    IdentifierName("GetHashCode")))))));

        // bool Equals(type other)
        yield return CreateEqualsMethod(IdentifierName(ctx.Symbol.Name));

        foreach (var impl in ctx.ImplementingTypes)
        {
            var type = impl.Type;
            var implName = TypeNameGlobal(type);

            // public bool Equals(impl other)
            yield return CreateEqualsMethod(implName);
        }
    }

    /// <summary>
    /// Returns a method declaration for the Equals method comparing the current type with the specified type.
    /// </summary>
    private static MethodDeclarationSyntax CreateEqualsMethod(TypeSyntax typeName)
    {
        return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.BoolKeyword)),
                Identifier("Equals"))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(Identifier("other"))
                            .WithType(typeName))))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ReturnStatement(
                            BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                IdentifierName("Handle"),
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("other"),
                                    IdentifierName("Handle")))))));
    }

}

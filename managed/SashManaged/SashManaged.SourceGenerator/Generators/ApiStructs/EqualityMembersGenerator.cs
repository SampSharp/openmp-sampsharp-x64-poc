using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.HelperSyntaxFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SashManaged.SourceGenerator.Generators.ApiStructs;

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
                            PredefinedType(Token(SyntaxKind.ObjectKeyword))))))
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
                                ParseTypeName("nint"),
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
                                TypeNameGlobal(Constants.PointerFQN),
                                SingleVariableDesignation(
                                    Identifier("other")))),
                        InvocationExpression(
                            IdentifierName("Equals"))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(IdentifierName("other")))))))));

        // public bool Equals(IPointer other)
        yield return MethodDeclaration(
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
                            .WithType(IdentifierName("IPointer")))))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ReturnStatement(
                            BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                IdentifierName("_handle"),
                                ParenthesizedExpression(
                                    BinaryExpression(
                                        SyntaxKind.CoalesceExpression,
                                        ConditionalAccessExpression(
                                            IdentifierName("other"),
                                            MemberBindingExpression(
                                                IdentifierName("Handle"))),
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("nint"),
                                            IdentifierName("Zero")))))))));

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

        //type == object
        yield return CreateOperator(
                SyntaxKind.EqualsEqualsToken,
                IdentifierName(ctx.Symbol.Name),
                PredefinedType(Token(SyntaxKind.ObjectKeyword)),
                CreateEqualsInvocationLhsRhs(false)
            );

        // type != object
        yield return CreateOperator(
                SyntaxKind.ExclamationEqualsToken,
                IdentifierName(ctx.Symbol.Name),
                PredefinedType(Token(SyntaxKind.ObjectKeyword)),
                CreateEqualsInvocationLhsRhs(true)
            );

        // object == type
        yield return CreateOperator(
                SyntaxKind.EqualsEqualsToken,
                PredefinedType(Token(SyntaxKind.ObjectKeyword)),
                IdentifierName(ctx.Symbol.Name),
                CreateEqualsInvocationRhsLhs(false)
            );

        // object != type
        yield return CreateOperator(
                SyntaxKind.ExclamationEqualsToken,
                PredefinedType(Token(SyntaxKind.ObjectKeyword)),
                IdentifierName(ctx.Symbol.Name),
                CreateEqualsInvocationRhsLhs(true)
            );

        // bool Equals(type other)
        yield return CreateEqualsMethod(IdentifierName(ctx.Symbol.Name));

        //type == type
        yield return CreateOperator(
                SyntaxKind.EqualsEqualsToken,
                IdentifierName(ctx.Symbol.Name),
                IdentifierName(ctx.Symbol.Name),
                CreateEqualsInvocationLhsRhs(false)
            );

        // type != type
        yield return CreateOperator(
                SyntaxKind.ExclamationEqualsToken,
                IdentifierName(ctx.Symbol.Name),
                IdentifierName(ctx.Symbol.Name),
                CreateEqualsInvocationLhsRhs(true)
            );

        foreach (var type in ctx.ImplementingTypes)
        {
            var implName = TypeNameGlobal(type);

            // public bool Equals(impl other)
            yield return CreateEqualsMethod(implName);

            //type == impl
            yield return CreateOperator(
                    SyntaxKind.EqualsEqualsToken,
                    IdentifierName(ctx.Symbol.Name),
                    implName,
                    CreateEqualsInvocationLhsRhs(false)
                );

            // type != impl
            yield return CreateOperator(
                    SyntaxKind.ExclamationEqualsToken,
                    IdentifierName(ctx.Symbol.Name),
                    implName,
                    CreateEqualsInvocationLhsRhs(true)
                );

            // impl == type
            yield return CreateOperator(
                    SyntaxKind.EqualsEqualsToken,
                    implName,
                    IdentifierName(ctx.Symbol.Name),
                    CreateEqualsInvocationRhsLhs(false)
                );

            // impl != type
            yield return CreateOperator(
                    SyntaxKind.ExclamationEqualsToken,
                    implName,
                    IdentifierName(ctx.Symbol.Name),
                    CreateEqualsInvocationRhsLhs(true)
                );
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

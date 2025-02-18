﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators.ApiStructs;

public static class CreationMembersGenerator
{
    /// <summary>
    /// Returns members for the creation of values. This includes the handle field and property, the constructor,
    /// FromHandle methods, and implicit/explicit conversion operators.
    /// </summary>
    public static IEnumerable<MemberDeclarationSyntax> GenerateCreationMembers(StructStubGenerationContext ctx)
    {
        // private readonly field _handle;
        yield return GenerateHandleField();

        // .ctor(nint handle)
        yield return GenerateConstructor(ctx);

        // public nint Handle => _handle;
        yield return GenerateHandleProperty();

        if (ctx.IsComponent)
        {
            yield return GenerateFromHandleMethod(ctx, Constants.ComponentInterfaceFQN);
        }
        if (ctx.IsExtension)
        {
            yield return GenerateFromHandleMethod(ctx, Constants.ExtensionInterfaceFQN);
        }
    }

    private static PropertyDeclarationSyntax GenerateHandleProperty()
    {
        return PropertyDeclaration(ParseTypeName("nint"), "Handle")
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithExpressionBody(ArrowExpressionClause(IdentifierName("_handle")))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static FieldDeclarationSyntax GenerateHandleField()
    {
        return FieldDeclaration(VariableDeclaration(ParseTypeName("nint"), SingletonSeparatedList(VariableDeclarator("_handle"))))
            .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword)));
    }

    private static ConstructorDeclarationSyntax GenerateConstructor(StructStubGenerationContext ctx)
    {
        return ConstructorDeclaration(Identifier(ctx.Symbol.Name))
            .WithParameterList(ParameterList(
                SingletonSeparatedList(
                    Parameter(Identifier("handle")).WithType(ParseTypeName("nint"))
                )))
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithBody(Block(
                SingletonList(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression, 
                            IdentifierName("_handle"),
                            IdentifierName("handle"))))));
    }

    private static MethodDeclarationSyntax GenerateFromHandleMethod(StructStubGenerationContext ctx, string genericInterfaceFQN)
    {
        return MethodDeclaration(
                ctx.Type,
                Identifier("FromHandle"))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.StaticKeyword)))
            .WithExplicitInterfaceSpecifier(
                ExplicitInterfaceSpecifier(
                    GenericName(
                            IdentifierGlobal(genericInterfaceFQN))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList(ctx.Type)))))
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(
                                Identifier("handle"))
                            .WithType(
                                ParseTypeName("nint")))))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ReturnStatement(
                            ObjectCreationExpression(ctx.Type)
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                IdentifierName("handle")))))))));
    }
}

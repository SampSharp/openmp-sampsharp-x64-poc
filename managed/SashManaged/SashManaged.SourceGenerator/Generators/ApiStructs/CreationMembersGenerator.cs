using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SashManaged.SourceGenerator.Generators.ApiStructs;

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

        foreach (var type in ctx.ImplementingTypes)
        {
            var implName = TypeNameGlobal(type);

            //  public static implicit operator type(impl value)
            yield return GenerateCastToBaseType(ctx, implName);
            
            // public static explicit operator impl(type value)
            yield return GenerateCastFromBaseType(ctx, implName);
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

    private static ConversionOperatorDeclarationSyntax GenerateCastFromBaseType(StructStubGenerationContext ctx, TypeSyntax implName)
    {
        return ConversionOperatorDeclaration(
                Token(SyntaxKind.ExplicitKeyword),
                IdentifierName(ctx.Symbol.Name))
            .WithModifiers(
                TokenList(Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.StaticKeyword)))
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(
                                Identifier("value"))
                            .WithType(
                                implName))))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ReturnStatement(
                            ObjectCreationExpression(
                                    IdentifierName(ctx.Symbol.Name))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("value"),
                                                    IdentifierName("Handle"))))))))));
    }

    private static ConversionOperatorDeclarationSyntax GenerateCastToBaseType(StructStubGenerationContext ctx, TypeSyntax implName)
    {
        return ConversionOperatorDeclaration(
                Token(SyntaxKind.ImplicitKeyword),
                implName)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.StaticKeyword)))
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(
                                Identifier("value"))
                            .WithType(
                                IdentifierName(ctx.Symbol.Name)))))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ReturnStatement(
                            ObjectCreationExpression(
                                    implName)
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("value"),
                                                    IdentifierName("Handle"))))))))));
    }

    private static MethodDeclarationSyntax GenerateFromHandleMethod(StructStubGenerationContext ctx, string genericInterfaceFQN)
    {
        return MethodDeclaration(
                IdentifierName(ctx.Symbol.Name),
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
                                SingletonSeparatedList<TypeSyntax>(
                                    IdentifierName(ctx.Symbol.Name))))))
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
                            ObjectCreationExpression(
                                    IdentifierName(ctx.Symbol.Name))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                IdentifierName("handle")))))))));
    }
}

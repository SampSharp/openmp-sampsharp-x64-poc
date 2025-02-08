using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Models;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators.ApiStructs;

public static class CastMembersGenerator
{
    public static IEnumerable<MemberDeclarationSyntax> GenerateCastMembers(StructStubGenerationContext ctx)
    {
        var isFirst = true;
        foreach (var type in ctx.ImplementingTypes)
        {
            BlockSyntax block;

            if (isFirst)
            {
                block = Block(SingletonList<StatementSyntax>(
                        ReturnStatement(
                            ObjectCreationExpression(
                                    TypeNameGlobal(type))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("value"),
                                                    IdentifierName("Handle")))))))));
            }
            else
            {
                var func = GenerateExternFunctionCast(ctx, type);

                var invoke = InvocationExpression(
                        IdentifierName("__PInvoke"))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("value"),
                                        IdentifierName("Handle"))))));

                var ret = ReturnStatement(
                    ObjectCreationExpression(
                            TypeNameGlobal(type))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(invoke)))));

                block = Block(
                    List<StatementSyntax>([
                        ret,
                        func
                    ]));
            }
            yield return ConversionOperatorDeclaration(
                    Token(SyntaxKind.ExplicitKeyword),
                    TypeNameGlobal(type))
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
                .WithBody(block);

            isFirst = false;
        }
    }

    
    private static LocalFunctionStatementSyntax GenerateExternFunctionCast(StructStubGenerationContext ctx, ITypeSymbol target)
    {
        return HelperSyntaxFactory.GenerateExternFunction(
            library: ctx.Library, 
            externName: $"cast_{ctx.Symbol.Name}_to_{target.Name}",
            externReturnType: IntPtrType, 
            parameters: [new HelperSyntaxFactory.ParamForwardInfo("ptr", IntPtrType, RefKind.None)]);
    }

}
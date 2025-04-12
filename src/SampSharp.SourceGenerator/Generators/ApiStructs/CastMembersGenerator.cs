using System.Collections.Generic;
using System.Linq;
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
        foreach (var impl in ctx.ImplementingTypes)
        {
            var type = impl.Type;
            BlockSyntax block;

            if (isFirst)
            {
                block = Block(SingletonList<StatementSyntax>(
                        ReturnStatement(
                            ObjectCreationExpression(type.Syntax)
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
                if (impl.CastPath.Length == 1)
                {
                    var func = GenerateExternFunctionCast(ctx, type.Symbol);

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
                        ObjectCreationExpression(type.Syntax)
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
                else
                {
                    var cast = impl.CastPath.Aggregate((ExpressionSyntax)IdentifierName("value"), (current, c) => CastExpression(c.Syntax, current));

                    block = Block(SingletonList<StatementSyntax>(
                        ReturnStatement(cast)));
                }
            }
            yield return ConversionOperatorDeclaration(
                    Token(SyntaxKind.ExplicitKeyword),
                    type.Syntax)
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
                                    ctx.Type))))
                .WithLeadingTrivia(
                    TriviaFactory.DocsOpCast(ctx.Type, impl.Type.Syntax))
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
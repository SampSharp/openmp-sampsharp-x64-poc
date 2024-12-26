using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Marshalling;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.StatementFactory;

namespace SashManaged.SourceGenerator.SyntaxFactories;

public static class HelperSyntaxFactory
{
    public static LocalFunctionStatementSyntax GenerateExternFunction(
        string library,
        string externName,
        TypeSyntax externReturnType, 
        IEnumerable<ParamForwardInfo> parameters, 
        params ParameterSyntax[] parametersPrefix)
    {
        var externParameters = ToParameterListSyntax(parametersPrefix, parameters);

        return LocalFunctionStatement(externReturnType, "__PInvoke")
            .WithModifiers(TokenList(          
                Token(SyntaxKind.StaticKeyword),
                Token(SyntaxKind.UnsafeKeyword),
                Token(SyntaxKind.ExternKeyword)
            ))
            .WithParameterList(externParameters)
            .WithAttributeLists(
                SingletonList(
                    AttributeFactory.DllImport(library, externName)))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
            .WithLeadingTrivia(Comment(MarshallingCodeGenDocumentation.COMMENT_P_INVOKE));
    }

    public static ArgumentSyntax WithParameterRefToken(ArgumentSyntax argument, IParameterSymbol parameter)
    {
        switch (parameter.RefKind)
        {
            case RefKind.Ref:
                return argument.WithRefKindKeyword(Token(SyntaxKind.RefKeyword));
            case RefKind.Out:
                return argument.WithRefKindKeyword(Token(SyntaxKind.OutKeyword));
            default:
                return argument;
        }
    }

    public static BlockSyntax CreateEqualsInvocationLhsRhs(bool logicalNot)
    {
        return CreateThisEqualsThat(logicalNot, "lhs", "rhs");
    }

    public static BlockSyntax CreateEqualsInvocationRhsLhs(bool logicalNot)
    {
        return CreateThisEqualsThat(logicalNot, "rhs", "lhs");
    }

    public static OperatorDeclarationSyntax CreateOperator(SyntaxKind operatorToken, TypeSyntax lhsType, TypeSyntax rhsType, BlockSyntax block)
    {
        return OperatorDeclaration(
                PredefinedType(
                    Token(SyntaxKind.BoolKeyword)),
                Token(operatorToken))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.StaticKeyword)
                ))
            .WithParameterList(
                ParameterList(
                    SeparatedList(new []{
                        Parameter(Identifier("lhs"))
                            .WithType(lhsType),
                        Parameter(Identifier("rhs"))
                            .WithType(rhsType)
                    })))
            .WithBody(block);
    }

    private static BlockSyntax CreateThisEqualsThat(bool logicalNot, string @this, string that)
    {
        var invocation = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(@this),
                    IdentifierName("Equals")))
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(
                        Argument(
                            IdentifierName(that)))));

        if (logicalNot)
        {
            return Block(SingletonList<StatementSyntax>(
                ReturnStatement(
                    PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        invocation))));
        }

        return Block(SingletonList<StatementSyntax>(
            ReturnStatement(invocation)));
    }
}
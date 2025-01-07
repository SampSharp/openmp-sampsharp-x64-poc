﻿using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling;
using SampSharp.SourceGenerator.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.HelperSyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators.Marshalling;

public class ApiEventDelegateMarshallingGenerator() : MarshallingGeneratorBase(MarshalDirection.UnmanagedToManaged)
{
    private const string LocalHandler = "handler";
    public ExpressionSyntax GenerateDelegateExpression(MarshallingStubGenerationContext ctx)
    {
        ExpressionSyntax expr;
        if (!ctx.RequiresMarshalling)
        {
            expr = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(LocalHandler),
                IdentifierName(ctx.Symbol.Name));
        }
        else
        {
            var parameters = ToParameterListSyntax([], ctx.Parameters.Select(x => ToForwardInfo(x.Symbol, x.MarshallerShape, false)));

            expr = ParenthesizedExpression(
                ParenthesizedLambdaExpression()
                    .WithParameterList(
                        parameters)
                    .WithBlock(
                        GetMarshallingBlock(ctx)));
        }

        return CastExpression(
            IdentifierName($"{ctx.Symbol.Name}_"),
            expr);
    }

    protected override ExpressionSyntax GetInvocation(MarshallingStubGenerationContext ctx)
    {
        return InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(LocalHandler),
                        IdentifierName(ctx.Symbol.Name)))
                .WithArgumentList(
                    ArgumentList(
                        SeparatedList(
                            GetInvocationArguments(ctx))));
    }
}
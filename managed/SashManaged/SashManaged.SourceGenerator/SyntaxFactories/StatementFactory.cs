using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Marshalling;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.SyntaxFactories;

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

    // TODO: move to helper
    public static ParameterListSyntax ToParameterListSyntax(ParameterSyntax first, MethodStubGenerationContext ctx)
    {
        return ToParameterListSyntax([first], ctx.Parameters.Select(x => ToForwardInfo(x.Symbol, x.MarshallerShape)));
    }
    
    public static ParameterListSyntax ToParameterListSyntax(ImmutableArray<IParameterSymbol> parameters)
    {
        return ToParameterListSyntax([], parameters.Select(x => ToForwardInfo(x, null)));
    }

    public static ParameterListSyntax ToParameterListSyntax(ParameterSyntax[] prefix, IEnumerable<ParamForwardInfo> parameters)
    {
        return ParameterList(
            SeparatedList(prefix)
                .AddRange(
                    parameters
                        .Select(parameter => Parameter(Identifier(parameter.Name))
                            .WithType(parameter.Type)
                            .WithModifiers(GetRefTokens(parameter.RefKind)))));
    }

    public static ParamForwardInfo ToForwardInfo(IParameterSymbol symbol, IMarshallerShape marshallerShape)
    {
        return new ParamForwardInfo(symbol.Name, marshallerShape?.GetNativeType() ?? TypeSyntaxFactory.TypeNameGlobal(symbol.Type), symbol.RefKind);
    }

    public record struct ParamForwardInfo(string Name, TypeSyntax Type, RefKind RefKind);

    private static SyntaxTokenList GetRefTokens(RefKind refKind)
    {
        return refKind switch
        {
            RefKind.Ref => TokenList(Token(SyntaxKind.RefKeyword)),
            RefKind.Out => TokenList(Token(SyntaxKind.OutKeyword)),
            _ => default
        };
    }
}
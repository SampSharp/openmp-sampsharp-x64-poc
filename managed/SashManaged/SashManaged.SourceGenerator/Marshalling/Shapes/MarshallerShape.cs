﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling.Shapes;

public abstract class MarshallerShape(string nativeTypeName, string marshallerTypeName) : IMarshallerShape
{
    protected string NativeTypeName => nativeTypeName;
    protected string MarshallerTypeName => marshallerTypeName;

    public virtual bool RequiresLocal => true;

    public virtual TypeSyntax GetNativeType()
    {
        return ParseTypeName(nativeTypeName);
    }

    public virtual SyntaxList<StatementSyntax> Setup(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual FixedStatementSyntax? Pin(IParameterSymbol? parameterSymbol)
    {
        return null;
    }

    public virtual SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual ArgumentSyntax GetArgument(ParameterStubGenerationContext ctx)
    {
        ExpressionSyntax expr = IdentifierName($"__{ctx.Symbol.Name}_native");

        if (ctx.Symbol.RefKind is RefKind.In or RefKind.RefReadOnlyParameter)
        {
            expr = PrefixUnaryExpression(SyntaxKind.AddressOfExpression, expr);
        }

        return HelperSyntaxFactory.WithPInvokeParameterRefToken(Argument(expr), ctx.Symbol);
    }

    protected static string GetManagedVar(IParameterSymbol? parameterSymbol)
    {
        return parameterSymbol?.Name ?? "__retVal";
    }

    public virtual SyntaxList<StatementSyntax> GuaranteedUnmarshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    protected static string GetUnmanagedVar(IParameterSymbol? parameterSymbol)
    {
        return $"__{parameterSymbol?.Name ?? "retVal"}_native";
    }
}
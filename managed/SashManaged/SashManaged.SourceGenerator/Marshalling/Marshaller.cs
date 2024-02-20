using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public abstract class Marshaller(string nativeTypeName, string marshallerTypeName) : IMarshaller
{
    public virtual TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol)
    {
        return ParseTypeName(nativeTypeName);
    }

    public virtual SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }
    
    public virtual SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }
    
    protected static string Managed(IParameterSymbol parameterSymbol)
    {
        return parameterSymbol?.Name ?? "__retVal";
    }

    protected static string Unmanaged(IParameterSymbol parameterSymbol)
    {
        return $"__{(parameterSymbol?.Name ?? "__retVal")}_native";
    }

    protected SyntaxList<StatementSyntax> InvokeAndAssign(string left, string right, string method)
    {
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(left),
                    InvokeWithArgument(method, right))));
    }

    protected InvocationExpressionSyntax InvokeWithArgument(string method, string argument)
    {
        return InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(marshallerTypeName),
                    IdentifierName(method)
                )
            )
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(
                        Argument(IdentifierName(argument))
                    )
                )
            );
    }
}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public abstract class Marshaller : IMarshaller
{
    public abstract TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol);

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
    
    public virtual SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    protected static SyntaxList<StatementSyntax> InvokeAndAssign(string toValue, string fromValue, string marshallerType, string marshallerMethod)
    {
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(toValue),
                    InvokeWithArgument(marshallerType, marshallerMethod, fromValue))));
    }

    protected static InvocationExpressionSyntax InvokeWithArgument(string marshallerType, string marshallerMethod, string argument)
    {
        return InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(marshallerType),
                    IdentifierName(marshallerMethod)
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
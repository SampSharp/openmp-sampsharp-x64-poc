using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling.Shapes.Stateless;

public abstract class StatelessMarshallerShape(string nativeTypeName, string marshallerTypeName) : MarshallerShape(nativeTypeName, marshallerTypeName)
{
    protected SyntaxList<StatementSyntax> InvokeAndAssign(string local, string method, string argument)
    {
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(local),
                    InvokeWithArgument(method, argument))));
    }

    protected InvocationExpressionSyntax InvokeWithArgument(string method, string argument)
    {
        return InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(MarshallerTypeName),
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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling.Stateful;

/// <summary>
/// Stateful Unmanaged->Managed with Guaranteed Unmarshalling
/// </summary>
public class StatefulUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(string nativeTypeName, string marshallerTypeName) : StatefulUnmanagedToManagedMarshallerShape(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol)
    {
        return SyntaxFactory.List<StatementSyntax>();
    }

    public override SyntaxList<StatementSyntax> GuaranteedUnmarshal(IParameterSymbol? parameterSymbol)
    {
        // managed = marshaller.ToManagedFinally();
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(GetManagedVar(parameterSymbol)),
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(GetMarshallerVar(parameterSymbol)),
                            SyntaxFactory.IdentifierName("ToManagedFinally"))))));
    }
}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling.Stateful;

/// <summary>
/// Stateful Unmanaged->Managed
/// </summary>
public class StatefulUnmanagedToManagedMarshallerShape(string nativeTypeName, string marshallerTypeName) : StatefulMarshallerShape(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol? parameterSymbol)
    {
        // marshaller.FromUnmanaged(unmanaged);
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName("FromUnmanaged")))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    IdentifierName(GetUnmanagedVar(parameterSymbol))))))));
    }

    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol)
    {
        // managed = marshaller.ToManaged();
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(GetManagedVar(parameterSymbol)),
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName("ToManaged"))))));
    }

    public override SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol? parameterSymbol)
    {
        // marshaller.Free();
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(GetMarshallerVar(parameterSymbol)),
                        IdentifierName("Free")))));
    }
}
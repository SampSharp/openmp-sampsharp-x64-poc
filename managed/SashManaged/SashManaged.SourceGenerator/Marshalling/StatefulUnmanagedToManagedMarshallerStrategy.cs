using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public class StatefulUnmanagedToManagedMarshallerStrategy(string nativeTypeName, string marshallerTypeName) : Marshaller(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter)
    {
        // scoped type marshaller = new();
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(MarshallerTypeName),
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier($"__{parameter.Name}_native_marshaller"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ImplicitObjectCreationExpression()
                                    )
                                )
                        )
                    )
                )
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ScopedKeyword)))
        );
    }

    public override SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol parameterSymbol)
    {
        // marshaller.FromUnmanaged(unmanaged);
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(GetMarshallerVar(parameterSymbol)),
                            SyntaxFactory.IdentifierName("FromUnmanaged")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(GetUnmanagedVar(parameterSymbol))))))));
    }

    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        // managed = marshaller.ToManaged();
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(GetManagedVar(parameterSymbol)),
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(GetMarshallerVar(parameterSymbol)),
                            SyntaxFactory.IdentifierName("ToManaged"))))));
    }

    public override SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol parameterSymbol)
    {
        // marshaller.Free();
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(GetMarshallerVar(parameterSymbol)),
                        SyntaxFactory.IdentifierName("Free")))));
    }
}
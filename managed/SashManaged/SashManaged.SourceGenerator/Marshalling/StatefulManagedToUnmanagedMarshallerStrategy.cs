using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public class StatefulManagedToUnmanagedMarshallerStrategy(string nativeTypeName, string marshallerTypeName, bool notify) : Marshaller(nativeTypeName, marshallerTypeName)
{
    // public struct ManagedToNative // Can be ref struct
    // {
    //     TODO: public ref TIgnored GetPinnableReference(); // Result pinned for ToUnmanaged call and Invoke, but not used otherwise.
    //     TODO: public static ref TOther GetPinnableReference(TManaged managed); // Optional. Can throw exceptions. Result pinnned and passed to Invoke.
    // }

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

    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        // marshaller.FromManaged(managed);
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(GetMarshallerVar(parameterSymbol)),
                            SyntaxFactory.IdentifierName("FromManaged")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(GetManagedVar(parameterSymbol))))))));
    }

    public override SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol parameterSymbol)
    {
        // native = marshaller.ToUnmanaged();
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(GetUnmanagedVar(parameterSymbol)),
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(GetMarshallerVar(parameterSymbol)),
                            SyntaxFactory.IdentifierName("ToUnmanaged"))))));
    }

    public override SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol parameterSymbol)
    {
        if (!notify)
        {
            return base.NotifyForSuccessfulInvoke(parameterSymbol);
        }

        // marshaller.OnInvoked();
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(GetMarshallerVar(parameterSymbol)),
                        SyntaxFactory.IdentifierName("OnInvoked")))));
    }

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol parameterSymbol)
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
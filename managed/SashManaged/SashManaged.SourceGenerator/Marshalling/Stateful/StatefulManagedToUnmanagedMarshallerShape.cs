using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling.Stateful;

public class StatefulManagedToUnmanagedMarshallerShape(string nativeTypeName, string marshallerTypeName, bool notify) : StatefulMarshallerShape(nativeTypeName, marshallerTypeName)
{
    // public struct ManagedToNative // Can be ref struct
    // {
    //     TODO: public ref TIgnored GetPinnableReference(); // Result pinned for ToUnmanaged call and Invoke, but not used otherwise.
    //     TODO: public static ref TOther GetPinnableReference(TManaged managed); // Optional. Can throw exceptions. Result pinnned and passed to Invoke.
    // }

    public override SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter)
    {
        // scoped type marshaller = new();
        return SingletonList<StatementSyntax>(
            LocalDeclarationStatement(
                    VariableDeclaration(
                        IdentifierName(MarshallerTypeName),
                        SingletonSeparatedList(
                            VariableDeclarator(Identifier($"__{parameter.Name}_native_marshaller"))
                                .WithInitializer(
                                    EqualsValueClause(
                                        ImplicitObjectCreationExpression()
                                    )
                                )
                        )
                    )
                )
                .WithModifiers(TokenList(Token(SyntaxKind.ScopedKeyword)))
        );
    }

    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        // marshaller.FromManaged(managed);
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName("FromManaged")))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    IdentifierName(GetManagedVar(parameterSymbol))))))));
    }

    public override SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol parameterSymbol)
    {
        // native = marshaller.ToUnmanaged();
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(GetUnmanagedVar(parameterSymbol)),
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName("ToUnmanaged"))))));
    }

    public override SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol parameterSymbol)
    {
        if (!notify)
        {
            return base.NotifyForSuccessfulInvoke(parameterSymbol);
        }

        // marshaller.OnInvoked();
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(GetMarshallerVar(parameterSymbol)),
                        IdentifierName("OnInvoked")))));
    }

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol parameterSymbol)
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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling.Shapes.Stateful;

/// <summary>
/// Stateful Managed->Unmanaged
/// </summary>
public class StatefulManagedToUnmanagedMarshallerShape(string nativeTypeName, string marshallerTypeName, bool notify, bool pinMarshaller) : StatefulMarshallerShape(nativeTypeName, marshallerTypeName)
{
    public override FixedStatementSyntax? Pin(IParameterSymbol? parameterSymbol)
    {
        if (!pinMarshaller)
        {
            return null;
        }

        return FixedStatement(
                VariableDeclaration(
                        PointerType(
                            PredefinedType(
                                Token(SyntaxKind.VoidKeyword))))
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(
                                    Identifier($"__{parameterSymbol?.Name}_native__unused"))
                                .WithInitializer(
                                    EqualsValueClause(
                                        IdentifierName(GetMarshallerVar(parameterSymbol)))))),
                Block());
    }

    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
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

    public override SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol? parameterSymbol)
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

    public override SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol? parameterSymbol)
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

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
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
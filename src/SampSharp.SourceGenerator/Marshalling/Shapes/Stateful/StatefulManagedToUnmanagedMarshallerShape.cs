using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;

/// <summary>
/// Stateful Managed->Unmanaged
/// </summary>
public class StatefulManagedToUnmanagedMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType, bool notify, bool pinMarshaller, MarshalDirection direction) : StatefulMarshallerShape(nativeType, marshallerType, direction)
{
    public override FixedStatementSyntax? Pin(IParameterSymbol? parameterSymbol)
    {//
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
                                    Identifier(GetNativeExtraVar(parameterSymbol, "unused")))
                                .WithInitializer(
                                    EqualsValueClause(
                                        IdentifierName(GetMarshallerVar(parameterSymbol)))))),
                Block());
    }

    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {//
        // marshaller.FromManaged(managed);
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName(ShapeConstants.MethodFromManaged)))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    IdentifierName(GetManagedVar(parameterSymbol))))))));
    }

    public override SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol? parameterSymbol)
    {//
        // native = marshaller.ToUnmanaged();
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(GetNativeVar(parameterSymbol)),
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName(ShapeConstants.MethodToUnmanaged))))));
    }

    public override SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol? parameterSymbol)
    {//
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
                        IdentifierName(ShapeConstants.MethodOnInvoked)))));
    }

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
    {//
        // marshaller.Free();
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(GetMarshallerVar(parameterSymbol)),
                        IdentifierName(ShapeConstants.MethodFree)))));
    }
}
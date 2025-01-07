using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;

/// <summary>
/// Stateful Unmanaged->Managed
/// </summary>
public class StatefulUnmanagedToManagedMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType, MarshalDirection direction) : StatefulMarshallerShape(nativeType, marshallerType, direction)
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
                            IdentifierName(ShapeConstants.MethodFromUnmanaged)))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    IdentifierName(GetNativeVar(parameterSymbol))))))));
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
                            IdentifierName(ShapeConstants.MethodToManaged))))));
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
                        IdentifierName(ShapeConstants.MethodFree)))));
    }
}
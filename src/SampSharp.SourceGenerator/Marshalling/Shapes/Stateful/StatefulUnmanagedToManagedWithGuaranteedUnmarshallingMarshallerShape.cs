using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;

/// <summary>
/// Stateful Unmanaged->Managed with Guaranteed Unmarshalling
/// </summary>
public class StatefulUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType) : StatefulUnmanagedToManagedMarshallerShape(nativeType, marshallerType)
{
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public override SyntaxList<StatementSyntax> GuaranteedUnmarshal(IParameterSymbol? parameterSymbol)
    {
        // managed = marshaller.ToManagedFinally();
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(GetManagedVar(parameterSymbol)),
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName(ShapeConstants.MethodToManagedFinally))))));
    }
}
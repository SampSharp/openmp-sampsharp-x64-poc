using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling.Stateful;

/// <summary>
/// Stateful Managed->Unmanaged with Caller Allocated Buffer
/// </summary>
public class StatefulManagedToUnmanagedWithBufferMarshallerShape(string nativeTypeName, string marshallerTypeName, bool notify) : StatefulManagedToUnmanagedMarshallerShape(nativeTypeName, marshallerTypeName, notify)
{
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {
        // marshaller.FromManaged(managed, stackalloc byte[type.BufferSize]);
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName("FromManaged")))
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList(
                                new[] {
                                    Argument(IdentifierName(GetManagedVar(parameterSymbol))),
                                    Argument(StackAllocArrayCreationExpression(
                                        ArrayType(
                                                PredefinedType(Token(SyntaxKind.ByteKeyword)))
                                            .WithRankSpecifiers(
                                                SingletonList(
                                                    ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName(MarshallerTypeName),
                                                            IdentifierName("BufferSize"))))))))
                                })))));
    }
}
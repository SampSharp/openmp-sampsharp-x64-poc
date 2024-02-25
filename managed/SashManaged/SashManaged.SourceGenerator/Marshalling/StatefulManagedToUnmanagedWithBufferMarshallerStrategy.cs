using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public class StatefulManagedToUnmanagedWithBufferMarshallerStrategy(string nativeTypeName, string marshallerTypeName, bool notify) : StatefulManagedToUnmanagedMarshallerStrategy(nativeTypeName, marshallerTypeName, notify)
{
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        // marshaller.FromManaged(managed, stackalloc byte[type.BufferSize]);
        return SyntaxFactory.SingletonList<StatementSyntax>(
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(GetMarshallerVar(parameterSymbol)),
                            SyntaxFactory.IdentifierName("FromManaged")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList(
                                new[] {
                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(GetManagedVar(parameterSymbol))),
                                    SyntaxFactory.Argument(SyntaxFactory.StackAllocArrayCreationExpression(
                                        SyntaxFactory.ArrayType(
                                                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ByteKeyword)))
                                            .WithRankSpecifiers(
                                                SyntaxFactory.SingletonList(
                                                    SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression, 
                                                            SyntaxFactory.IdentifierName(MarshallerTypeName), 
                                                            SyntaxFactory.IdentifierName("BufferSize"))))))))
                                })))));
    }
}
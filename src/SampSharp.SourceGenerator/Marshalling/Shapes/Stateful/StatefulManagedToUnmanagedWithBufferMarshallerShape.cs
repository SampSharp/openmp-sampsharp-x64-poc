using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;

/// <summary>
/// Stateful Managed->Unmanaged with Caller Allocated Buffer
/// </summary>
public class StatefulManagedToUnmanagedWithBufferMarshallerShape(string nativeTypeName, string marshallerTypeName, bool notify, bool pinMarshaller) : StatefulManagedToUnmanagedMarshallerShape(nativeTypeName, marshallerTypeName, notify, pinMarshaller)
{
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {
        var bufferVar = $"__{parameterSymbol?.Name ?? "retVal"}_native__buffer";

        return List<StatementSyntax>([
            // global::System.Span<byte> __varName_native__buffer = stackalloc byte[MarshallerType.BufferSize];
            LocalDeclarationStatement(
                VariableDeclaration(
                        QualifiedName(
                            AliasQualifiedName(
                                IdentifierName(
                                    Token(SyntaxKind.GlobalKeyword)),
                                IdentifierName("System")),
                            GenericName(
                                    Identifier("Span"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(SingletonSeparatedList<TypeSyntax>(
                                            PredefinedType(
                                                Token(SyntaxKind.ByteKeyword)))))))
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(
                                    Identifier(bufferVar))
                                .WithInitializer(
                                    EqualsValueClause(
                                        StackAllocArrayCreationExpression(
                                            ArrayType(
                                                    PredefinedType(
                                                        Token(SyntaxKind.ByteKeyword)))
                                                .WithRankSpecifiers(
                                                    SingletonList(
                                                        ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(
                                                                MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    IdentifierName(MarshallerTypeName),
                                                                    IdentifierName("BufferSize")))))))))))),
            
            // marshaller.FromManaged(managed, stackalloc byte[type.BufferSize]);
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
                                    Argument(IdentifierName(bufferVar))
                                }))))]);
    }
}
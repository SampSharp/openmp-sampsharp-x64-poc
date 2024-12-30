using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling.Shapes.Stateless;

/// <summary>
/// Stateless Managed->Unmanaged with Caller-Allocated Buffer
/// </summary>
public class StatelessManagedToUnmanagedWithCallerAllocatedBufferMarshallerShape(string nativeTypeName, string marshallerTypeName, bool hasFree) : StatelessMarshallerShape(nativeTypeName, marshallerTypeName)
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
            
            // MarshallerType.ConvertToUnmanaged(managed, __varName_native__buffer);
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(GetUnmanagedVar(parameterSymbol)),
                    InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(MarshallerTypeName),
                                IdentifierName("ConvertToUnmanaged")
                            )
                        )
                        .WithArgumentList(
                            ArgumentList(
                                SeparatedList([
                                    Argument(IdentifierName(GetManagedVar(parameterSymbol))),
                                    Argument(IdentifierName(bufferVar))
                                ])
                            )
                        )))]);
    }

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
    {
        // type.Free(unmanaged);
        return !hasFree
            ? List<StatementSyntax>()
            : SingletonList<StatementSyntax>(
                ExpressionStatement(
                    InvokeWithArgument("Free", GetUnmanagedVar(parameterSymbol))));
    }
}
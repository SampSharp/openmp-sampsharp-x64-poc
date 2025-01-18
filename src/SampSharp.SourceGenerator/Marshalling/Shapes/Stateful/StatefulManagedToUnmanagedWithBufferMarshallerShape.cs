using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;

/// <summary>
/// Stateful Managed->Unmanaged with Caller Allocated Buffer
/// </summary>
public class StatefulManagedToUnmanagedWithBufferMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType, bool notify, bool pinMarshaller, MarshalDirection direction) : StatefulManagedToUnmanagedMarshallerShape(nativeType, marshallerType, notify, pinMarshaller, direction)
{
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {//
        var bufferVar = GetNativeExtraVar(parameterSymbol, "buffer");
        
        return List<StatementSyntax>([
            // global::System.Span<byte> __varName_native__buffer = stackalloc byte[MarshallerType.BufferSize];
            LocalDeclarationStatement(
                VariableDeclaration(TypeSyntaxFactory.SpanOfBytes)
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
                                                                    IdentifierName(ShapeConstants.PropertyBufferSize)))))))))))),
            
            // marshaller.FromManaged(managed, stackalloc byte[type.BufferSize]);
            ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(GetMarshallerVar(parameterSymbol)),
                            IdentifierName(ShapeConstants.MethodFromManaged)))
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList([
                                Argument(IdentifierName(GetManagedVar(parameterSymbol))),
                                    Argument(IdentifierName(bufferVar))
                            ]))))]);
    }
}
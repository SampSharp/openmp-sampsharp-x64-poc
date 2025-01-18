using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.ShapeGenerators;

public class StatefulUnmanagedToManaged(IMarshalShapeGenerator innerGenerator) : IMarshalShapeGenerator
{
    public bool UsesNativeIdentifier => true;

    public TypeSyntax GetNativeType(IdentifierStubContext context)
    {
        return context.NativeType!.TypeName;
    }

    public IEnumerable<StatementSyntax> Generate(MarshalPhase phase, IdentifierStubContext context)
    {
        return phase switch
        {
            MarshalPhase.Setup => Setup(context),
            MarshalPhase.UnmarshalCapture => UnmarshalCapture(context),
            MarshalPhase.Unmarshal => Unmarshal(context),
            MarshalPhase.CleanupCalleeAllocated => CleanupCalleeAllocated(context),
            _ => innerGenerator.Generate(phase, context)
        };
    }

    private static IEnumerable<StatementSyntax> Setup(IdentifierStubContext context)
    {
        // TODO: not always scoped
        // scoped type marshaller = new();
        yield return LocalDeclarationStatement(
                VariableDeclaration(
                    context.MarshallerType!.TypeName,
                    SingletonSeparatedList(
                        VariableDeclarator(Identifier(context.GetMarshallerId()))
                            .WithInitializer(
                                EqualsValueClause(
                                    ImplicitObjectCreationExpression()
                                )
                            )
                    )
                )
            )
            .WithModifiers(TokenList(Token(SyntaxKind.ScopedKeyword)));
    }

    private static IEnumerable<StatementSyntax> CleanupCalleeAllocated(IdentifierStubContext context)
    {
        // marshaller.Free();
        yield return ExpressionStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(context.GetMarshallerId()),
                    IdentifierName(ShapeConstants.MethodFree))));
    }

    private static IEnumerable<StatementSyntax> Unmarshal(IdentifierStubContext context)
    {
        // managed = marshaller.ToManaged();
        yield return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(context.GetManagedId()),
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(context.GetMarshallerId()),
                        IdentifierName(ShapeConstants.MethodToManaged)))));
    }

    private static IEnumerable<StatementSyntax> UnmarshalCapture(IdentifierStubContext context)
    {
        // marshaller.FromUnmanaged(unmanaged);
        yield return ExpressionStatement(
            InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(context.GetMarshallerId()),
                        IdentifierName(ShapeConstants.MethodFromUnmanaged)))
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(
                                IdentifierName(context.GetNativeId()))))));
    }
}
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.ShapeGenerators;

public class StatefulManagedToUnmanaged(IMarshalShapeGenerator innerGenerator) : IMarshalShapeGenerator
{
    public bool UsesNativeIdentifier => true;

    public TypeSyntax GetNativeType(IdentifierStubContext context)
    {
        return TypeSyntaxFactory.TypeNameGlobal(context.MarshallerMembers!.StatefulToUnmanagedMethod!.ReturnType);
    }

    public IEnumerable<StatementSyntax> Generate(MarshalPhase phase, IdentifierStubContext context)
    {
        return phase switch
        {
            MarshalPhase.Setup => Setup(context),
            MarshalPhase.Marshal => Marshal(context),
            MarshalPhase.PinnedMarshal => PinnedMarshal(context),
            _ => innerGenerator.Generate(phase, context)
        };
    }

    private static IEnumerable<StatementSyntax> PinnedMarshal(IdentifierStubContext context)
    {
        // native = marshaller.ToUnmanaged();
        yield return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(context.GetNativeVar()),
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(context.GetMarshallerVar()),
                        IdentifierName(ShapeConstants.MethodToUnmanaged)))));
    }

    private static IEnumerable<StatementSyntax> Marshal(IdentifierStubContext context)
    {
        // marshaller.FromManaged(managed);
        yield return ExpressionStatement(
            InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(context.GetMarshallerVar()),
                        IdentifierName(ShapeConstants.MethodFromManaged)))
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(
                                IdentifierName(context.GetManagedVar()))))));
    }

    private static IEnumerable<StatementSyntax> Setup(IdentifierStubContext context)
    {
        // TODO: not always scoped
        // scoped type marshaller = new();
        yield return LocalDeclarationStatement(
                VariableDeclaration(
                    IdentifierName(context.Marshaller!.TypeName),
                    SingletonSeparatedList(
                        VariableDeclarator(Identifier(context.GetMarshallerVar()))
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
}
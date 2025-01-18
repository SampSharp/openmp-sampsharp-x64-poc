using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.V2.ShapeGenerators;

internal class StatelessUnmanagedToManagedGuaranteed(IMarshalShapeGenerator innerGenerator) : IMarshalShapeGenerator
{
    public bool UsesNativeIdentifier => true;

    public TypeSyntax GetNativeType(IdentifierStubContext context)
    {
        return TypeSyntaxFactory.TypeNameGlobal(context.MarshallerMembers!.StatelessConvertToManagedFinallyMethod!.Parameters[0].Type);
    }

    public IEnumerable<StatementSyntax> Generate(MarshalPhase phase, IdentifierStubContext context)
    {
        return phase switch
        {
            MarshalPhase.GuaranteedUnmarshal => GuaranteedUnmarshal(context),
            _ => innerGenerator.Generate(phase, context)
        };
    }

    private static IEnumerable<StatementSyntax> GuaranteedUnmarshal(IdentifierStubContext context)
    {
        // managed = Marshaller.ConvertToManagedFinally(unmanaged);
        yield return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(context.GetManagedVar()),
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(context.Marshaller!.TypeName),
                            IdentifierName(ShapeConstants.MethodConvertToManagedFinally)
                        )
                    )
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(IdentifierName(context.GetNativeVar()))
                            )
                        )
                    )));
    }
}
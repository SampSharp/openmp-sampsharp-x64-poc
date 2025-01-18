using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.ShapeGenerators;

public class StatelessFree(IMarshalShapeGenerator innerGenerator) : IMarshalShapeGenerator
{
    public bool UsesNativeIdentifier => innerGenerator.UsesNativeIdentifier;

    public TypeSyntax GetNativeType(IdentifierStubContext context)
    {
        return innerGenerator.GetNativeType(context);
    }

    public IEnumerable<StatementSyntax> Generate(MarshalPhase phase, IdentifierStubContext context)
    {
        return phase switch
        {
            MarshalPhase.CleanupCallerAllocated => CleanupCallerAllocated(context),
            _ => innerGenerator.Generate(phase, context)
        };
    }

    private static IEnumerable<StatementSyntax> CleanupCallerAllocated(IdentifierStubContext context)
    {
        yield return ExpressionStatement(
            InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        context.MarshallerType!.TypeName,
                        IdentifierName(ShapeConstants.MethodFree)
                    )
                )
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(IdentifierName(context.GetNativeId()))
                        )
                    )
                ));
    }
}
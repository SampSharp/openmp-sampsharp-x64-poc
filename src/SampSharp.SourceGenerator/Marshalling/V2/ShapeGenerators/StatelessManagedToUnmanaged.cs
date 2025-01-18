using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.V2.ShapeGenerators;

public class StatelessManagedToUnmanaged(IMarshalShapeGenerator innerGenerator) : IMarshalShapeGenerator
{
    public bool UsesNativeIdentifier => true;

    public TypeSyntax GetNativeType(IdentifierStubContext context)
    {
        return TypeSyntaxFactory.TypeNameGlobal(context.MarshallerMembers!.StatelessConvertToUnmanagedWithBufferMethod?.ReturnType 
                                                ?? context.MarshallerMembers!.StatelessConvertToUnmanagedMethod!.ReturnType);
    }

    public IEnumerable<StatementSyntax> Generate(MarshalPhase phase, IdentifierStubContext context)
    {
        return phase switch
        {
            MarshalPhase.Marshal => Marshal(context),
            _ => innerGenerator.Generate(phase, context)
        };
    }

    private static IEnumerable<StatementSyntax> Marshal(IdentifierStubContext context)
    {
        // native = Marshaller.ConvertToUnmanaged(managed);
        yield return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(context.GetNativeVar()),
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(context.Marshaller!.TypeName),
                            IdentifierName(ShapeConstants.MethodConvertToUnmanaged)
                        )
                    )
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(IdentifierName(context.GetManagedVar()))
                            )
                        )
                    )));
    }
}
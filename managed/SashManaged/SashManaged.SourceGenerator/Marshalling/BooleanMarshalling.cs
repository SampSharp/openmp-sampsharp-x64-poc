using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public class BooleanMarshalling : Marshaller
{
    public static BooleanMarshalling Instance { get; } = new();

    public override TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol)
    {
        return ParseTypeName($"global::{Constants.BlittableBooleanFQN}");
    }

    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign($"__{parameterSymbol.Name}_native", parameterSymbol.Name, "global::SashManaged.BooleanMarshaller", "ConvertToUnmanaged");
    }
    
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        if (parameterSymbol == null)
        {
            return InvokeAndAssign("__retVal", "__retVal_native", "global::SashManaged.BooleanMarshaller", "ConvertToManaged");
        }

        return InvokeAndAssign(parameterSymbol.Name, $"__{parameterSymbol.Name}_native", "global::SashManaged.BooleanMarshaller", "ConvertToManaged");
    }

    private static SyntaxList<StatementSyntax> InvokeAndAssign(string toValue, string fromValue, string marshallerType, string marshallerMethod)
    {
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(toValue),
                    InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(marshallerType),
                                IdentifierName(marshallerMethod)
                            )
                        )
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(IdentifierName(fromValue))
                                )
                            )
                        ))));
    }
}
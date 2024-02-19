using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public class StringMarshalling : Marshaller
{
    public static StringMarshalling Instance { get; } = new();

    public override TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol)
    {
        return ParseTypeName($"global::{Constants.StringViewFQN}");
    }
    
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign($"__{parameterSymbol.Name}_native", parameterSymbol.Name, "global::SashManaged.StringViewMarshaller", "ConvertToUnmanaged");
    }
    
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        if (parameterSymbol == null)
        {
            return InvokeAndAssign("__retVal", "__retVal_native", "global::SashManaged.StringViewMarshaller", "ConvertToManaged");
        }

        return InvokeAndAssign(parameterSymbol.Name, $"__{parameterSymbol.Name}_native", "global::SashManaged.StringViewMarshaller", "ConvertToManaged");
    }

    public override SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol)
    {
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                InvokeWithArgument("global::SashManaged.StringViewMarshaller", "Free", $"__{parameterSymbol.Name}_native")));
    }

    private static SyntaxList<StatementSyntax> InvokeAndAssign(string toValue, string fromValue, string marshallerType, string marshallerMethod)
    {
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(toValue),
                    InvokeWithArgument(marshallerType, marshallerMethod, fromValue))));
    }

    private static InvocationExpressionSyntax InvokeWithArgument(string marshallerType, string marshallerMethod, string argument)
    {
        return InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(marshallerType),
                    IdentifierName(marshallerMethod)
                )
            )
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(
                        Argument(IdentifierName(argument))
                    )
                )
            );
    }
    // old below

    // public override SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter)
    // {
    //     // TODO to const
    //     var marshallerType = ParseTypeName("global::SashManaged.StringViewMarshaller.ManagedToUnmanagedIn");
    //
    //     // TODO: to singleton list
    //     return List(new StatementSyntax[]{
    //         LocalDeclarationStatement(
    //                 VariableDeclaration(
    //                     marshallerType,
    //                     SingletonSeparatedList(
    //                         VariableDeclarator(Identifier($"__{parameter.Name}_native_marshaller"))
    //                             .WithInitializer(
    //                                 EqualsValueClause(
    //                                     ImplicitObjectCreationExpression()
    //                                 )
    //                             )
    //                     )
    //                 )
    //             )
    //             .WithModifiers(TokenList(Token(SyntaxKind.ScopedKeyword))),
    //     });
    // }

    public override SyntaxList<StatementSyntax> ManagedToUnmanaged(IParameterSymbol parameter)
    {
        var marshallerType = ParseTypeName("global::SashManaged.StringViewMarshaller.ManagedToUnmanagedIn");

        var stackAlloc = StackAllocArrayCreationExpression(
            ArrayType(
                    PredefinedType(Token(SyntaxKind.ByteKeyword)))
                .WithRankSpecifiers(
                    SingletonList(
                        ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(
                                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, marshallerType, IdentifierName("BufferSize"))
                            )
                        )
                    )
                )
        );
        return List(new StatementSyntax[]
        {
            ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName($"__{parameter.Name}_native_marshaller"),
                            IdentifierName("FromManaged")
                        )
                    )
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList(new[]{
                                Argument(IdentifierName(parameter.Name)),
                                Argument(stackAlloc)
                            })
                        )
                    )
            ),
            ExpressionStatement(
                AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName($"__{parameter.Name}_native"),
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName($"__{parameter.Name}_native_marshaller"),
                            IdentifierName("ToUnmanaged")
                        )
                    )
                )
            )
        });
    }

    public override SyntaxList<StatementSyntax> Free(IParameterSymbol parameter)
    {
        return List(new StatementSyntax[]
        {
            ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName($"__{parameter.Name}_native_marshaller"),
                        IdentifierName("Free")
                    )
                )
            )
        });
    }

    public override ArgumentSyntax GetArgument(IParameterSymbol parameter)
    {
        return WithParameterRefKind(Argument(IdentifierName($"__{parameter.Name}_native")), parameter);
    }

    public override ExpressionSyntax UnmanagedToManaged(ExpressionSyntax expression)
    {
        return InvocationExpression(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("global::SashManaged.StringViewMarshaller"),
                IdentifierName("ConvertToManaged")
            )
        )
        .WithArgumentList(
            ArgumentList(
                SingletonSeparatedList(
                    Argument(expression)
                )
            )
        );
    }
}
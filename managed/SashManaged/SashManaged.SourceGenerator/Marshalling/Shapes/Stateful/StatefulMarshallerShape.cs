using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using SashManaged.SourceGenerator.Marshalling.Shapes;

namespace SashManaged.SourceGenerator.Marshalling.Shapes.Stateful;

public abstract class StatefulMarshallerShape(string nativeTypeName, string marshallerTypeName) : MarshallerShape(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Setup(IParameterSymbol? parameterSymbol)
    {
        // TODO: if not ref, then not scoped

        // scoped type marshaller = new();
        return SingletonList<StatementSyntax>(
            LocalDeclarationStatement(
                    VariableDeclaration(
                        IdentifierName(MarshallerTypeName),
                        SingletonSeparatedList(
                            VariableDeclarator(Identifier(GetMarshallerVar(parameterSymbol)))
                                .WithInitializer(
                                    EqualsValueClause(
                                        ImplicitObjectCreationExpression()
                                    )
                                )
                        )
                    )
                )
                .WithModifiers(TokenList(Token(SyntaxKind.ScopedKeyword)))
        );
    }

    protected static string GetMarshallerVar(IParameterSymbol? parameterSymbol)
    {
        return $"__{parameterSymbol?.Name ?? "retVal"}_native_marshaller";
    }
}
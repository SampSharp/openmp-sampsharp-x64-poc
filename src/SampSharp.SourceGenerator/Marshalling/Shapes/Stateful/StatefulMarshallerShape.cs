using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateful;

public abstract class StatefulMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType, MarshalDirection direction) : MarshallerShape(nativeType, marshallerType, direction)
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
}
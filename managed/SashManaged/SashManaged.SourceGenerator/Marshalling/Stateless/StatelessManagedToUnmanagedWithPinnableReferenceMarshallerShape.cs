using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace SashManaged.SourceGenerator.Marshalling.Stateless;

/// <summary>
/// Stateless Managed->Unmanaged with pinnable reference.
/// </summary>
public class StatelessManagedToUnmanagedWithPinnableReferenceMarshallerShape(string nativeTypeName, string marshallerTypeName) : StatelessMarshallerShape(nativeTypeName, marshallerTypeName)
{
    public override bool RequiresLocal => false;
    
    public override FixedStatementSyntax Pin(IParameterSymbol? parameterSymbol)
    {
        return FixedStatement(VariableDeclaration(
                PointerType(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword))))
            .WithVariables(
                SingletonSeparatedList(
                    VariableDeclarator(
                            Identifier(GetUnmanagedVar(parameterSymbol)))
                        .WithInitializer(
                            EqualsValueClause(
                                PrefixUnaryExpression(
                                    SyntaxKind.AddressOfExpression,
                                    InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                ParseTypeName(MarshallerTypeName),
                                                IdentifierName("GetPinnableReference")))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList(
                                                    Argument(
                                                        IdentifierName(GetManagedVar(parameterSymbol))))))))))), Block());
    }

    public override ArgumentSyntax GetArgument(ParameterStubGenerationContext ctx)
    {
        return HelperSyntaxFactory.WithParameterRefToken(Argument(CastExpression(GetNativeType(), IdentifierName($"__{ctx.Symbol.Name}_native"))), ctx.Symbol);
    }
}
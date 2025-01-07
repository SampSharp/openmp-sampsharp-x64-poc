using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.HelperSyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

/// <summary>
/// Stateless Managed->Unmanaged with pinnable reference.
/// </summary>
public class StatelessManagedToUnmanagedWithPinnableReferenceMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType, MarshalDirection direction) : StatelessMarshallerShape(nativeType, marshallerType, direction)
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
                            Identifier(GetNativeVar(parameterSymbol)))
                        .WithInitializer(
                            EqualsValueClause(
                                PrefixUnaryExpression(
                                    SyntaxKind.AddressOfExpression,
                                    InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                ParseTypeName(MarshallerTypeName),
                                                IdentifierName(ShapeConstants.MethodGetPinnableReference)))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList(
                                                    Argument(
                                                        IdentifierName(GetManagedVar(parameterSymbol))))))))))), Block());
    }

    public override ArgumentSyntax GetArgument(ParameterStubGenerationContext ctx)
    {
        
        return WithPInvokeParameterRefToken(
            Argument(
                CastExpression(
                    GetNativeType(), 
                    IdentifierName(GetNativeVar(ctx.Symbol)))),
            ctx.Symbol);
    }
}
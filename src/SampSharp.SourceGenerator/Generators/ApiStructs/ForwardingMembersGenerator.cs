using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.HelperSyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators.ApiStructs;

public static class ForwardingMembersGenerator
{
    /// <summary>
    /// Returns members for implementing the types specified in the CodeGen attribute.
    /// </summary>
    public static SyntaxList<MemberDeclarationSyntax> GenerateImplementingTypeMembers(StructStubGenerationContext ctx)
    {
        var result = List<MemberDeclarationSyntax>();

        foreach (var implementingType in ctx.ImplementingTypes)
        {
            var implementingMethods = implementingType.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => !x.IsStatic && x.MethodKind == MethodKind.Ordinary && x.Name != "Equals");

            foreach (var implementingMethod in implementingMethods)
            {
                var method = MethodDeclaration(
                        TypeNameGlobal(implementingMethod), 
                        implementingMethod.Name)
                    .WithParameterList(ToParameterListSyntax(implementingMethod.Parameters))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)));

                SimpleNameSyntax memberName = IdentifierName(implementingMethod.Name);
                if (implementingMethod.TypeParameters.Length > 0)
                {
                    method = method.WithTypeParameterList(
                        TypeParameterList(
                            SeparatedList(
                                implementingMethod.TypeParameters.Select(x => TypeParameter(x.Name)))));

                    memberName = GenericName(
                            Identifier(implementingMethod.Name))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SeparatedList<TypeSyntax>(
                                    implementingMethod.TypeParameters.Select(x => IdentifierName(x.Name)))));
                }

                method = method.WithConstraintClauses(
                    List(
                        implementingMethod.TypeParameters
                            .Select(x => TypeParameterConstraintClause(IdentifierName(x.Name))
                                .WithConstraints(
                                    SeparatedList(
                                        x.ConstraintTypes.Select(y => (TypeParameterConstraintSyntax)ToTypeConstraint(y)))))));    

                var invocation =  
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ObjectCreationExpression(
                                    TypeNameGlobal(implementingType))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                IdentifierName("_handle"))))),
                            memberName))
                        .WithArgumentList(
                            ArgumentList(
                                SeparatedList(
                                    implementingMethod.Parameters.Select(symbol => WithPInvokeParameterRefToken(Argument(IdentifierName(symbol.Name)), symbol.RefKind)))));
                
                if (implementingMethod.ReturnsVoid)
                {
                    method = method.WithBody(
                        Block(
                            SingletonList(
                                ExpressionStatement(invocation))));
                }
                else if (implementingMethod.ReturnsByRef || implementingMethod.ReturnsByRefReadonly)
                {
                    method = method.WithBody(
                        Block(
                            SingletonList(
                                ReturnStatement(
                                    RefExpression(invocation)))));
                }
                else
                {
                    method = method.WithBody(
                        Block(
                            SingletonList(
                                ReturnStatement(invocation))));
                }

                result = result.Add(method);
            }
        }

        return result;
    }

    private static TypeConstraintSyntax ToTypeConstraint(ITypeSymbol typeSymbol)
    {
        var typeSyntax = ParseTypeName(typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        return TypeConstraint(typeSyntax);
    }
}

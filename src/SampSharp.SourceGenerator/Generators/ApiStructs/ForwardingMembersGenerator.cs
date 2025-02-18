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

        foreach (var impl in ctx.ImplementingTypes)
        {
            var implementingType = impl.Type;
            // only forward public ordinary methods, excluding Equals
            var implementingMethods = implementingType.Symbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => !x.IsStatic && x.MethodKind == MethodKind.Ordinary && x.Name != "Equals" && x.DeclaredAccessibility == Accessibility.Public)
                .ToList();

            for (var index = 0; index < implementingMethods.Count; index++)
            {
                var implementingMethod = implementingMethods[index];


                var method = MethodDeclaration(
                        TypeNameGlobal(implementingMethod, true), 
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
                                    ToConstraintList(x)))));    

                var target = ParenthesizedExpression(
                    CastExpression(
                        implementingType.Syntax,
                        ThisExpression()));

                var invocation =  
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            target,
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

    private static SeparatedSyntaxList<TypeParameterConstraintSyntax> ToConstraintList(ITypeParameterSymbol x)
    {
        var result = SeparatedList<TypeParameterConstraintSyntax>();

        if (x.HasReferenceTypeConstraint)
        {
            result = result.Add(ClassOrStructConstraint(SyntaxKind.ClassConstraint));
        }
        
        if (x.HasValueTypeConstraint)
        {
            result = result.Add(ClassOrStructConstraint(SyntaxKind.StructConstraint));
        }

        if (x.HasNotNullConstraint)
        {
            result = result.Add(TypeConstraint(IdentifierName("notnull")));
        }
        
        if (x.HasUnmanagedTypeConstraint)
        {
            result = result.Add(TypeConstraint(IdentifierName("unmanaged")));
        }

        result = result.AddRange(x.ConstraintTypes.Select(ToTypeConstraint));
        
        if (x.HasConstructorConstraint)
        {
            result = result.Add(ClassOrStructConstraint(SyntaxKind.ConstructorConstraint));
        }

        return result;
    }

    private static TypeParameterConstraintSyntax ToTypeConstraint(ITypeSymbol typeSymbol)
    {

        var typeSyntax = ParseTypeName(typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        return TypeConstraint(typeSyntax);
    }
}

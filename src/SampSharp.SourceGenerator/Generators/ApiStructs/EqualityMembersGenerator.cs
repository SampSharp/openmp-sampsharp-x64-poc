using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators.ApiStructs;

public static class EqualityMembersGenerator
{
    public static IEnumerable<MemberDeclarationSyntax> GenerateEqualityMembers(StructStubGenerationContext ctx)
    {
        // public override int GetHashCode()
        yield return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.IntKeyword)),
                Identifier("GetHashCode"))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.OverrideKeyword)
                ))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ReturnStatement(
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("_handle"),
                                    IdentifierName("GetHashCode")))))));
    }
}

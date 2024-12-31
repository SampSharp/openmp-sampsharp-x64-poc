﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator;

[Generator]
public class NumberedTypeSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var structDeclarations = context.SyntaxProvider.ForAttributeWithMetadataName(Constants.NumberedTypeGeneratorAttributeFQN,
            predicate: static (s, _) => s is StructDeclarationSyntax, transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx));

        var compilationAndStructs = context.CompilationProvider.Combine(structDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndStructs, static (spc, source) => Execute(source.Left, source.Right, spc));
    }

    private static StructData GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context)
    {
        var attributes = new AttributeData[context.Attributes.Length];

        for (var i = 0; i < context.Attributes.Length; i++)
        {
            var attribute = context.Attributes[i];
            var fieldName = attribute.ConstructorArguments[0].Value as string ?? "unknown";
            var value = (int)attribute.ConstructorArguments[1].Value!;

            attributes[i] = new AttributeData(fieldName, value);
        }

        return new StructData((StructDeclarationSyntax)context.TargetNode, attributes);
    }

    private static void Execute(Compilation compilation, ImmutableArray<StructData> datas, SourceProductionContext context)
    {
        foreach (var data in datas)
        {
            foreach (var attribute in data.Attributes)
            {

                var model = compilation.GetSemanticModel(data.Declaration.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(data.Declaration)!;

                var newStructName = GetNewStructName(symbol.Name, attribute.Value);
                var source = GenerateNewStructSource(symbol, newStructName, attribute.Field, attribute.Value, compilation);

                context.AddSource($"{newStructName}.g.cs", source);
            }
        }
    }

    private static string GetNewStructName(string originalName, int value)
    {
        var numberIndex = originalName.Length - 1;
        while (numberIndex >= 0 && char.IsDigit(originalName[numberIndex]))
        {
            numberIndex--;
        }

        var baseName = originalName.Substring(0, numberIndex + 1);
        return $"{baseName}{value}";
    }

    private static string GenerateNewStructSource(INamedTypeSymbol symbol, string newStructName, string fieldName, int value, Compilation compilation)
    {
        // Get the original struct declaration
        if (symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is not StructDeclarationSyntax structDeclarationSyntax)
        {
            throw new InvalidOperationException("Unable to retrieve the struct declaration syntax.");
        }

        // Get the struct with fully qualified types
        var qualifiedStruct = FullyQualifyTypes(structDeclarationSyntax, compilation);

        // Rename the struct
        var updatedStruct = qualifiedStruct.WithIdentifier(Identifier(newStructName));

        // Enable nullable if required
        if (compilation.Options.NullableContextOptions == NullableContextOptions.Enable)
        {
            
            updatedStruct = updatedStruct.WithOpenBraceToken(
                updatedStruct.OpenBraceToken.WithTrailingTrivia(TriviaList(
                Trivia(
                    NullableDirectiveTrivia(
                        Token(SyntaxKind.EnableKeyword),
                        true)))));
        }



        // Replace the constant field value
        var members = updatedStruct.Members.Select(member =>
        {
            if (member is FieldDeclarationSyntax fieldDeclaration && fieldDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword) &&
                fieldDeclaration.Declaration.Variables.Any(v => v.Identifier.Text == fieldName))
            {
                // Update the constant value
                var updatedVariables = fieldDeclaration.Declaration.Variables.Select(variable =>
                {
                    if (variable.Identifier.Text == fieldName)
                    {
                        return variable.WithInitializer(
                            EqualsValueClause(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value))));
                    }

                    return variable;
                });

                var updatedDeclaration = fieldDeclaration.Declaration.WithVariables(SeparatedList(updatedVariables));
                return fieldDeclaration.WithDeclaration(updatedDeclaration);
            }

            if (member is ConstructorDeclarationSyntax constructor)
            {
                // Replace the type name in constructors
                var updatedConstructor = constructor.WithIdentifier(Identifier(newStructName));
                return updatedConstructor;
            }

            return member; // Keep other members unchanged
        });

        // Update the struct with modified members
        updatedStruct = updatedStruct.WithMembers(List(members));
        
        // Ensure attributes have fully qualified names
        var updatedAttributes = updatedStruct.AttributeLists.Select(attributeList =>
            AttributeList(SeparatedList(
                attributeList.Attributes.Where(attribute => !attribute.Name.ToString().Contains("NumberedTypeGenerator")))))
            .Where(x => x.Attributes.Count > 0);
        
        updatedStruct = updatedStruct.WithAttributeLists(List(updatedAttributes));

        // Generate the updated namespace and code
        var namespaceDeclaration = NamespaceDeclaration(ParseName(symbol.ContainingNamespace.ToDisplayString()))
            .AddMembers(updatedStruct)
            .NormalizeWhitespace();

        return namespaceDeclaration.ToFullString();
    }

    private static StructDeclarationSyntax FullyQualifyTypes(StructDeclarationSyntax structDeclaration, Compilation compilation)
    {
        // Traverse the original tree and resolve symbols
        var semanticModel = compilation.GetSemanticModel(structDeclaration.SyntaxTree);

        // Create a new rewriter that works on symbols tied to the original syntax tree
        var rewriter = new SimpleFullyQualifyRewriter(semanticModel);
        return (StructDeclarationSyntax)rewriter.Visit(structDeclaration);
    }


    private readonly record struct StructData(StructDeclarationSyntax Declaration, AttributeData[] Attributes);

    private readonly record struct AttributeData(string Field, int Value);

    private class SimpleFullyQualifyRewriter : CSharpSyntaxRewriter
    {
        private readonly SemanticModel _semanticModel;

        public SimpleFullyQualifyRewriter(SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
        }

        public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
        {
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol;

            if (symbol is ITypeSymbol { SpecialType: SpecialType.None } typeSymbol)
            {
                // Fully qualify the type
                var fullyQualifiedName = ParseName($"global::{typeSymbol.ToDisplayString()}");
                return fullyQualifiedName;
            }

            return base.VisitIdentifierName(node);
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            return ResolveMemberAccessSyntaxTree(node);
        }

        private MemberAccessExpressionSyntax ResolveMemberAccessSyntaxTree(MemberAccessExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax access)
            {
                return node.WithExpression(ResolveMemberAccessSyntaxTree(access));
            }

            if (node.Expression is IdentifierNameSyntax name)
            {
                var symbol = _semanticModel.GetSymbolInfo(name);
                if (symbol.Symbol is INamedTypeSymbol)
                {
                    return node.WithExpression(name.WithIdentifier(Identifier($"global::{symbol.Symbol}")));
                }
            }

            return node;
        }

        public override SyntaxNode VisitAttribute(AttributeSyntax node)
        {
            var symbol = _semanticModel.GetSymbolInfo(node.Name).Symbol;

            node = (AttributeSyntax)base.VisitAttribute(node)!;

            if (symbol != null)
            {
                node = node.WithName(ParseName($"global::{symbol.ContainingNamespace}.{node.Name}"));
            }

            return node;
        }
    }
}
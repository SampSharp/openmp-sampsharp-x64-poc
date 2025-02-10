using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SampSharp.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExtensionAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string ExtensionTypeFQN = "SampSharp.OpenMp.Core.Extension";
    public const string ExtensionAttributeTypeFQN = "SampSharp.OpenMp.Core.ExtensionAttribute";

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Analyzers.MissingExtensionAttribute];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var extensionType = context.Compilation.GetTypeByMetadataName(ExtensionTypeFQN);
        var extensionAttributeType = context.Compilation.GetTypeByMetadataName(ExtensionAttributeTypeFQN);

        if(extensionType == null || extensionAttributeType == null)
        {
            return;
        }

        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        if (!context.SemanticModel.IsBaseType(classDeclaration, extensionType))
        {
            return;
        }

        if(!context.SemanticModel.HasAttribute(classDeclaration, extensionAttributeType))
        {
            var diagnostic = Diagnostic.Create(
                Analyzers.MissingExtensionAttribute, 
                classDeclaration.Identifier.GetLocation(), 
                classDeclaration.Identifier.ToString());

            context.ReportDiagnostic(diagnostic);
        }
    }
}
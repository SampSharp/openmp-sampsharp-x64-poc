using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SampSharp.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExtensionAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SASH0001";
    private static readonly LocalizableString _title = "Missing 'ExtensionAttribute'";
    private static readonly LocalizableString _messageFormat = "Type {0} is missing the 'ExtensionAttribute'";
    private static readonly LocalizableString _description = "Type {0} must have the 'ExtensionAttribute'.";
    private const string Category = "Problem";

    private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor(
        DiagnosticId, _title, _messageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: _description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [_rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        if (!IsBaseTypeExtension(context, classDeclaration))
        {
            return;
        }

        if(!HasAttribute(classDeclaration, context.SemanticModel, "SampSharp.OpenMp.Core.ExtensionAttribute"))
        {
            var diagnostic = Diagnostic.Create(_rule, classDeclaration.GetLocation(), classDeclaration.Identifier.ToString());
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static bool HasAttribute(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, string fullAttributeName)
    {
        foreach (var attributeList in classDeclaration.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                var symbol = semanticModel.GetTypeInfo(attribute).Type;
                if (symbol == null)
                    continue;

                if (symbol.ToDisplayString() == fullAttributeName)
                    return true;
            }
        }

        return false;
    }

    private static bool IsBaseTypeExtension(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration)
    {
        var check = context.Compilation.GetTypeByMetadataName("SampSharp.OpenMp.Core.Extension");
        if (check == null)
        {
            return false;
        }

        if (classDeclaration.BaseList != null)
        {
            foreach (var baseTypeSyntax in classDeclaration.BaseList.Types)
            {
                var baseTypeSymbol = context.SemanticModel.GetTypeInfo(baseTypeSyntax.Type).Type;

                if (baseTypeSymbol == null)
                {
                    continue;
                }

                if(SymbolEqualityComparer.Default.Equals(baseTypeSymbol, check))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
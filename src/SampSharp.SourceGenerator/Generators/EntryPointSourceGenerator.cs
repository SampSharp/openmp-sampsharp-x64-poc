using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators;

[Generator]
public class EntryPointSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
                (node, _) => node is ClassDeclarationSyntax cls && cls.BaseList != null,
                (ctx, _) =>
                {
                    var classDeclaration = (ClassDeclarationSyntax)ctx.Node;
                    var semanticModel = ctx.SemanticModel;

                    if (semanticModel.GetDeclaredSymbol(classDeclaration) is INamedTypeSymbol classSymbol)
                    {
                        var interf = semanticModel.Compilation.GetTypeByMetadataName("SampSharp.OpenMp.Core.IStartup");
                        if (interf != null && classSymbol.AllInterfaces.Contains(interf))
                        {
                            return classDeclaration;
                        }
                    }

                    return null;
                })
            .Where(cls => cls != null)
            .Select((cls, _) => cls);

        context.RegisterSourceOutput(provider, Execute);
    }

    private static string GetFQN(ClassDeclarationSyntax classDeclaration)
    {
        // Start with the class name
        var className = classDeclaration.Identifier.Text;

        // Traverse upwards to find all namespaces
        var currentNode = classDeclaration.Parent;
        while (currentNode != null)
        {
            className = currentNode switch
            {
                BaseNamespaceDeclarationSyntax ns => ns.Name + "." + className,
                _ => className
            };

            currentNode = currentNode.Parent;
        }

        return className;
    }

    private void Execute(SourceProductionContext ctx, ClassDeclarationSyntax? syntax)
    {
        var source = Generate(syntax!);
        
        ctx.AddSource("EntryPoint.g.cs", source);
    }

    private static SourceText Generate(ClassDeclarationSyntax syntax)
    {
        var startup = TypeNameGlobal(GetFQN(syntax));

        var unit = CompilationUnit()
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    NamespaceDeclaration(ParseName("SampSharp.OpenMp.Core"))
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            ClassDeclaration("Interop")
                            .WithModifiers(
                                TokenList(
                                    Token(SyntaxKind.PublicKeyword), 
                                    Token(SyntaxKind.StaticKeyword)))
                            .WithMembers(
                                List<MemberDeclarationSyntax>([
                                    FieldDeclaration(
                                            VariableDeclaration(
                                                    startup)
                                            .WithVariables(
                                                SingletonSeparatedList(
                                                    VariableDeclarator(
                                                        Identifier("_startup"))
                                                    .WithInitializer(
                                                        EqualsValueClause(
                                                            ImplicitObjectCreationExpression())))))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PrivateKeyword),
                                                Token(SyntaxKind.StaticKeyword),
                                                Token(SyntaxKind.ReadOnlyKeyword))),
                                        FieldDeclaration(
                                            VariableDeclaration(
                                                TypeNameGlobal(Constants.StartupContextFQN))
                                            .WithVariables(
                                                SingletonSeparatedList(
                                                    VariableDeclarator(
                                                        Identifier("_context")))))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PrivateKeyword), 
                                                Token(SyntaxKind.StaticKeyword))),
                                        MethodDeclaration(
                                            PredefinedType(
                                                Token(SyntaxKind.VoidKeyword)),
                                            Identifier("Cleanup"))
                                        .WithAttributeLists(
                                            SingletonList(
                                                AttributeFactory.UnmanagedCallersOnly()))
                                        .WithModifiers(
                                            TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                                        .WithBody(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    ExpressionStatement(
                                                        ConditionalAccessExpression(
                                                            IdentifierName("_context"),
                                                            ConditionalAccessExpression(
                                                                MemberBindingExpression(
                                                                    IdentifierName("Cleanup")),
                                                                InvocationExpression(
                                                                    MemberBindingExpression(
                                                                        IdentifierName("Invoke"))))))))),
                                        MethodDeclaration(
                                            PredefinedType(
                                                Token(SyntaxKind.VoidKeyword)),
                                            Identifier("Initialize"))
                                        .WithAttributeLists(
                                            SingletonList(
                                                AttributeFactory.UnmanagedCallersOnly()))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword),
                                                Token(SyntaxKind.StaticKeyword)))
                                        .WithParameterList(
                                            ParameterList(
                                                SingletonSeparatedList(
                                                    Parameter(
                                                            Identifier("inf"))
                                                    .WithType(
                                                        QualifiedName(
                                                            QualifiedName(
                                                                QualifiedName(
                                                                    AliasQualifiedName(
                                                                        IdentifierName(
                                                                            Token(SyntaxKind.GlobalKeyword)),
                                                                        IdentifierName("SampSharp")),
                                                                    IdentifierName("OpenMp")),
                                                                IdentifierName("Core")),
                                                            IdentifierName("SampSharpInitParams"))))))
                                        .WithBody(
                                            Block(
                                                ExpressionStatement(
                                                    AssignmentExpression(
                                                        SyntaxKind.SimpleAssignmentExpression,
                                                        IdentifierName("_context"),
                                                        ObjectCreationExpression(
                                                                TypeNameGlobal(Constants.StartupContextFQN))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SingletonSeparatedList(
                                                                    Argument(
                                                                        IdentifierName(
                                                                            Identifier("inf")))))))),
                                                ExpressionStatement(
                                                    InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName("_startup"),
                                                            IdentifierName("Initialize")))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SingletonSeparatedList(
                                                                Argument(
                                                                    IdentifierName("_context")))))))),
                                        MethodDeclaration(
                                            PredefinedType(
                                                Token(SyntaxKind.VoidKeyword)),
                                            Identifier("Main"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword), 
                                                Token(SyntaxKind.StaticKeyword)))
                                        .WithBody(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    StatementFactory.Invoke(Constants.StartupContextFQN, "MainInfoProvider"))))
                                ]))))));

        var sourceText = unit.NormalizeWhitespace(elasticTrivia: true)
            .GetText(Encoding.UTF8);

        return sourceText;
    }


}
using System;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SashManaged.SourceGenerator
{
    [Generator]
    public class StructPtrCodeGen : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var structPovider = context.SyntaxProvider
                .CreateSyntaxProvider(predicate: IsStructPtrStub, transform: GetMapperDeclaration)
                .WithTrackingName("Syntax");

            context.RegisterSourceOutput(structPovider, (ctx, node) =>
            {
                if (node == null)
                {
                    return;
                }

                ctx.AddSource(node.Symbol.Name + ".g.cs", SourceText.From(Process(node), Encoding.UTF8));
            });
        }

        private static string Process(StructDecl node)
        {
            // TODO: visibility
            var sb = new StringBuilder();

            sb.Append($$"""
                        namespace {{node.Symbol.ContainingNamespace.ToDisplayString()}}
                        {
                            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
                            public partial struct {{node.Symbol.Name}}
                            {
                                private readonly nint _data;
                        """);

            foreach (var member in node.TypeDeclaration.Members)
            {
                if (member is not MethodDeclarationSyntax memberDeclaration || 
                    !member.Modifiers.Any(x => x.IsKind(SyntaxKind.PartialKeyword)))
                {
                    continue;
                }

                var isVoidReturn = IsVoid(memberDeclaration.ReturnType);
                var returnType = memberDeclaration.ReturnType.ToFullString();
                var methodName = memberDeclaration.Identifier.ToFullString();
                var proxyName = $"{node.Symbol.Name}_{FirstLower(methodName)}";
                sb.Append($$"""
                            [System.Runtime.InteropServices.DllImport("SampSharp", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
                            private static extern {{returnType}} {{proxyName}} ({{node.Symbol.Name}} ptr
                            """);

                if (memberDeclaration.ParameterList.Parameters.Count > 0)
                {
                    sb.Append($", {memberDeclaration.ParameterList.Parameters.ToFullString()}");
                }

                sb.AppendLine(");");

                sb.Append($$"""
                            public partial {{returnType}} {{methodName}} ({{memberDeclaration.ParameterList.Parameters.ToFullString()}})
                            {
                                {{(!isVoidReturn ? "return" : "")}} {{proxyName}} (
                                this {{(memberDeclaration.ParameterList.Parameters.Count > 0 ? ", " : "")}}
                                {{string.Join(", ", memberDeclaration.ParameterList.Parameters.Select(x => x.Identifier.ValueText))}}
                            );
                            }
                            """);
            }

            sb.Append("""
                            }
                        }
                        """);

            return sb.ToString();
        }

        private static string FirstLower(string value)
        {
            return $"{char.ToLowerInvariant(value[0])}{value.Substring(1)}";
        }

        private static bool IsStructPtrStub(SyntaxNode syntax, CancellationToken _) =>
            syntax is StructDeclarationSyntax
            {
                AttributeLists.Count: > 0
            } structDecl && structDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));

        private static StructDecl GetMapperDeclaration(GeneratorSyntaxContext ctx, CancellationToken cancellationToken)
        {
            var declaration = (StructDeclarationSyntax)ctx.Node;
            if (ctx.SemanticModel.GetDeclaredSymbol(declaration, cancellationToken) is not { } symbol)
                return null;

            return HasAttribute(symbol, "SashManaged.OpenMpAttribute") ? new StructDecl(symbol, declaration) : null;
        }

        private static bool IsVoid(TypeSyntax type)
        {
            return type is PredefinedTypeSyntax predefined && predefined.Keyword.IsKind(SyntaxKind.VoidKeyword);
        }

        private static bool HasAttribute(ISymbol symbol, string attributeName)
        {
            return symbol
                .GetAttributes()
                .Any(x =>
                    string.Equals(
                        x.AttributeClass?.ToDisplayString(_fullyQualifiedFormatWithoutGlobal),
                        attributeName,
                        StringComparison.Ordinal
                    )
                );
        }
        
        private static readonly SymbolDisplayFormat _fullyQualifiedFormatWithoutGlobal =
            SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining);

        private class StructDecl
        {
            public ISymbol Symbol { get; }
            public StructDeclarationSyntax TypeDeclaration { get; }
            public StructDecl(ISymbol symbol, StructDeclarationSyntax typeDeclaration)
            {
                Symbol = symbol;
                TypeDeclaration = typeDeclaration;
            }
        }
    }
}

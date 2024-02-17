using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SashManaged.SourceGenerator;

[Generator]
public class OpenMpHybridStringGeneratorCodeGen : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var structPovider = context.SyntaxProvider
            .CreateSyntaxProvider(predicate: IsPartialStruct, transform: GetStructDeclaration)
            .WithTrackingName("Syntax");

        context.RegisterSourceOutput(structPovider, (ctx, node) =>
        {
            if (node == null)
            {
                return;
            }

            ctx.AddSource(node.Symbol.Name + ".g.cs", SourceText.From(ProcessStruct(node), Encoding.UTF8));
        });

    }

    private static string ProcessStruct(StructDecl node)
    {
        var sb = new StringBuilder();

        var size = node.Size;
        sb.AppendLine($$"""
                        /// <auto-generator />
                        
                        #nullable enable
                        
                        namespace {{node.Symbol.ContainingNamespace.ToDisplayString()}}
                        {
                            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
                            {{node.TypeDeclaration.Modifiers}} struct {{node.Symbol.Name}}
                            {
                                private const int Length = {{size}};
                                
                                // First bit is 1 if dynamic and 0 if static; the rest are the length
                                [System.Runtime.InteropServices.FieldOffset(0)] private readonly Size _lenDynamic;
                                
                                //[FieldOffset(Size.Length)]
                                //private readonly byte* _ptr;
                                
                                [System.Runtime.InteropServices.FieldOffset(Size.Length)] [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = Length)]
                                private readonly byte[]? _static;
                                
                                public {{node.Symbol.Name}}(string inp)
                                {
                                    var q = System.Text.Encoding.UTF8.GetBytes(inp);
                                    if (q.Length < Length)
                                    {
                                        _static = new byte[Length];
                                
                                        q.CopyTo(_static, 0);
                                        _lenDynamic = new Size(new nint((long)inp.Length << 1));
                                    }
                                    else
                                        _static = null;
                                }
                                
                                public Span<byte> AsSpan()
                                {
                                    var value = _lenDynamic.Value.ToInt64();
                                    var flag = (value & 1) != 0;
                                    var length = value >> 1;
                                
                                    if (flag)
                                    {
                                        //return new Span<byte>(_ptr, (int)length);
                                    }
                                
                                    return new Span<byte>(_static, 0, (int)length);
                                }
                                
                                public override string ToString()
                                {
                                    return System.Text.Encoding.UTF8.GetString(AsSpan());
                                }
                            }
                        }
                        """);
        return sb.ToString();
    }

    private static bool IsPartialStruct(SyntaxNode syntax, CancellationToken _)
    {
        return syntax is StructDeclarationSyntax
        {
            AttributeLists.Count: > 0
        } structDecl && structDecl.IsPartial();
    }

    private static StructDecl GetStructDeclaration(GeneratorSyntaxContext ctx, CancellationToken cancellationToken)
    {
        var structDeclaration = (StructDeclarationSyntax)ctx.Node;
        if (ctx.SemanticModel.GetDeclaredSymbol(structDeclaration, cancellationToken) is not { } structSymbol)
            return null;

        if (!structSymbol.HasAttribute(Constants.HybridStringGeneratorAttributeFQN))
            return null;

        var size = (int)structSymbol.GetAttributes(Constants.HybridStringGeneratorAttributeFQN).Single().ConstructorArguments.First().Value!;

        return new StructDecl(structSymbol, structDeclaration, size);
    }

    private class StructDecl(ISymbol symbol, StructDeclarationSyntax typeDeclaration, int size)
    {
        public ISymbol Symbol { get; } = symbol;
        public StructDeclarationSyntax TypeDeclaration { get; } = typeDeclaration;
        public int Size { get; } = size;
    }
}
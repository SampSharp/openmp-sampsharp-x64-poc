using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.InteropServices;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator;

public static class AttributeFactory
{
    public static readonly string GENERATED_CODE_FQN = $"global::{typeof(GeneratedCodeAttribute).FullName}";
    public static readonly string SKIP_LOCALS_INIT_FQN = "global::System.Runtime.CompilerServices.SkipLocalsInitAttribute";
    public static readonly string DLL_IMPORT_FQN = $"global::{typeof(DllImportAttribute).FullName}";
    private static readonly string CALLING_CONVENTION_FQN = $"global::{typeof(CallingConvention).FullName}";

    public static AttributeListSyntax GeneratedCode()
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName();
        
        return AttributeList(
            SingletonSeparatedList(
                Attribute(
                        ParseName(GENERATED_CODE_FQN)) 
                    .WithArgumentList(
                        AttributeArgumentList(
                            SeparatedList(
                                new []{
                                    AttributeArgument(
                                        LiteralExpression(
                                            SyntaxKind.StringLiteralExpression, 
                                            Literal(assemblyName.Name))),
                                    AttributeArgument(
                                        LiteralExpression(
                                            SyntaxKind.StringLiteralExpression, 
                                            Literal(assemblyName.Version.ToString())))
                                }
                            )))));
    }

    public static AttributeListSyntax SkipLocalsInit()
    {
        return AttributeList(
            SingletonSeparatedList(
                Attribute(
                    ParseName(SKIP_LOCALS_INIT_FQN))));
    }

    public static AttributeListSyntax DllImport(string library, string entryPoint, string callingConvention = "Cdecl")
    {
        var conv = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression, 
            ParseTypeName(CALLING_CONVENTION_FQN), 
            IdentifierName(callingConvention));

        return AttributeList(
            SingletonSeparatedList(
                Attribute(ParseName(DLL_IMPORT_FQN),
                    AttributeArgumentList(
                        SeparatedList(new[] {
                            AttributeArgument(
                                LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(library))
                            ),
                            AttributeArgument(conv)
                                .WithNameEquals(NameEquals(nameof(DllImportAttribute.CallingConvention))),
                            AttributeArgument(
                                    LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(entryPoint))
                                )
                                .WithNameEquals(NameEquals(nameof(DllImportAttribute.EntryPoint))),
                            AttributeArgument(
                                    LiteralExpression(SyntaxKind.TrueLiteralExpression)
                                )
                                .WithNameEquals(NameEquals(nameof(DllImportAttribute.ExactSpelling)))
                        })
                    )
                )
            )
        );
    }
}
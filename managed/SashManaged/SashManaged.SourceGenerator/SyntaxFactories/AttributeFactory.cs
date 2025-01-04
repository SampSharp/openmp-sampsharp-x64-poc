using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.SyntaxFactories;

/// <summary>
/// Creates attributes syntax nodes.
/// </summary>
public static class AttributeFactory
{
    private static readonly string GeneratedCodeFQN = $"global::{typeof(GeneratedCodeAttribute).FullName}";
    private const string SkipLocalsInitFQN = "global::System.Runtime.CompilerServices.SkipLocalsInitAttribute";
    private static readonly string DllImportFQN = $"global::{typeof(DllImportAttribute).FullName}";
    private static readonly string CallingConventionFQN = $"global::{typeof(CallingConvention).FullName}";

    public static AttributeListSyntax GeneratedCode()
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName();

        return AttributeList(
            SingletonSeparatedList(
                Attribute(
                        ParseName(GeneratedCodeFQN))
                    .WithArgumentList(
                        AttributeArgumentList(
                            SeparatedList([
                                AttributeArgument(
                                    LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(assemblyName.Name))),
                                AttributeArgument(
                                    LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(assemblyName.Version.ToString())))
                            ])))));
    }

    public static AttributeListSyntax SkipLocalsInit()
    {
        return AttributeList(
            SingletonSeparatedList(
                Attribute(
                    ParseName(SkipLocalsInitFQN))));
    }

    public static AttributeListSyntax DllImport(string library, string entryPoint, CallingConvention callingConvention = CallingConvention.Cdecl)
    {
        var conv = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            ParseTypeName(CallingConventionFQN),
            IdentifierName(callingConvention.ToString()));

        return AttributeList(
            SingletonSeparatedList(
                Attribute(ParseName(DllImportFQN),
                    AttributeArgumentList(
                        SeparatedList([
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
                        ])
                    )
                )
            )
        );
    }
}
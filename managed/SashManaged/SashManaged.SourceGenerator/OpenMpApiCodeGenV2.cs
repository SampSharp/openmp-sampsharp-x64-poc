﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using SashManaged.SourceGenerator.Marshalling;

namespace SashManaged.SourceGenerator;

[Generator]
public class OpenMpApiCodeGenV2 : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var attributedStructs = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                Constants.ApiAttribute2FQN, 
                static (s, _) => s is StructDeclarationSyntax str && str.IsPartial(), 
                static(ctx, ct) => GetStructDeclaration(ctx, ct))
            .Where(x => x is not null);
        
        context.RegisterSourceOutput(attributedStructs, (ctx, info) =>
        {
            var node = info.Node;
            var symbol = info.Symbol;

            var structDeclaration = StructDeclaration(node.Identifier)
                .WithModifiers(node.Modifiers)
                .WithMembers(GenerateStructMembers(info));

            var unit = CompilationUnit()
                .AddMembers(NamespaceDeclaration(ParseName(symbol.ContainingNamespace.ToDisplayString()))
                    .AddMembers(structDeclaration))
                .WithLeadingTrivia(
                    Comment("// <auto-generated />"),
                    Trivia(PragmaWarningDirectiveTrivia(Token(SyntaxKind.DisableKeyword), CreateList(IdentifierName("CS8500")), true)));

            var sourceText = unit.NormalizeWhitespace(elasticTrivia:true)
                .GetText(Encoding.UTF8);
            
            ctx.AddSource($"{symbol.Name}.v2.g.cs", sourceText);
        });
    }

    private static SyntaxList<MemberDeclarationSyntax> GenerateStructMembers(StructDeclaration info)
    {
        return List(
            GenerateCommonStructMembers(info)
                .Concat(
                    info.Methods.Select(GenerateMethod)
                        .Where(x => x != null)
                )
            );
    }

    private static IEnumerable<MemberDeclarationSyntax> GenerateCommonStructMembers(StructDeclaration info)
    {
        var nint = ParseTypeName("nint");
        yield return FieldDeclaration(VariableDeclaration(nint, SingletonSeparatedList(VariableDeclarator("_handle"))))
            .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword)));

        yield return ConstructorDeclaration(Identifier(info.Symbol.Name))
            .WithParameterList(ParameterList(
                SingletonSeparatedList(
                Parameter(Identifier("handle")).WithType(nint)
                )))
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithBody(Block(
                SingletonList(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression, 
                            IdentifierName("_handle"),
                            IdentifierName("handle")
                            )
                        )
                    )
                )
            );

        yield return PropertyDeclaration(nint, "Handle")
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithExpressionBody(ArrowExpressionClause(IdentifierName("_handle")))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static MemberDeclarationSyntax GenerateMethod((MethodDeclarationSyntax methodDeclaration, IMethodSymbol methodSymbol) info)
    {
        var (node, symbol) = info;

        // Guard: cannot ref return a value that requires marshalling
        var returnMarshaller = GetMarshaller(symbol.ReturnType);
        if (returnMarshaller != null && symbol.ReturnsByRef)
        {
            // Cannot ref return a type that needs marshalling
            // TODO: diagnostic
            return null;
        }

        // TODO: ref return not yet supported
        if (symbol.ReturnsByRef)
        {
            return null;
        }

        var parameters = symbol.Parameters
            .Select(x => (parameter: x, marshaller: GetParameterMarshaller(x)))
            .ToList();

        var invocation = CreateInvocationAndMaybeMarshalling(symbol, parameters);
        
        // Extern P/Invoke
        var externReturnType = returnMarshaller?.ToMarshalledType(symbol.ReturnType) ?? 
                               ToTypeSyntax(symbol.ReturnType, symbol.ReturnsByRef, symbol.ReturnsByRefReadonly);

        var externFunction = CreateExternFunction(symbol, externReturnType, parameters);

        invocation = invocation.WithStatements(invocation.Statements.Add(externFunction));
     
        return MethodDeclaration(ToReturnTypeSyntax(symbol), node.Identifier)
            .WithModifiers(node.Modifiers)
            .WithParameterList(ToParameterListSyntax(symbol.Parameters))
            .WithAttributeLists(
                List(
                    new []{
                        AttributeList(
                            SingletonSeparatedList(
                                Attribute(
                                    ParseName("global::System.CodeDom.Compiler.GeneratedCodeAttribute")) // TODO const
                                    .WithArgumentList(
                                        AttributeArgumentList(
                                            SeparatedList(
                                                new []{
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression, 
                                                            Literal("Mylibrary"))), // TODO: from this assemlby
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression, 
                                                            Literal("0.0.1")))
                                                }
                                            ))))),
                        AttributeList(
                            SingletonSeparatedList(
                                Attribute(
                                    ParseName("global::System.Runtime.CompilerServices.SkipLocalsInitAttribute")))) // TODO const
                    }
                ))
            .WithBody(invocation);
    }

    private static BlockSyntax CreateInvocationAndMaybeMarshalling(IMethodSymbol symbol,
        List<(IParameterSymbol parameter, IMarshaller marshaller)> parameters)
    {
        var returnMarshaller = GetMarshaller(symbol.ReturnType);

        var marshallingRequired = returnMarshaller != null || parameters.Any(x => x.marshaller != null);

        return marshallingRequired 
            ? CreateInvokeWithMarshalling(symbol, parameters)
            : CreateInvokeWithoutMarshalling(symbol, parameters);
    }

    private static ArgumentSyntax GetArgumentForParameter(IParameterSymbol parameter, IMarshaller marshaller = null)
    {
        var identifier = parameter.Name;
        if (marshaller != null)
        {
            identifier = $"__{identifier}_native";
        }

        return WithParameterRefKind(Argument(IdentifierName(identifier)), parameter);
    }

    private static ArgumentSyntax WithParameterRefKind(ArgumentSyntax argument, IParameterSymbol parameter)
    {
        switch (parameter.RefKind)
        {
            case RefKind.Ref:
                return argument.WithRefKindKeyword(Token(SyntaxKind.RefKeyword));
            case RefKind.Out:
                return argument.WithRefKindKeyword(Token(SyntaxKind.OutKeyword));
            default:
                return argument;
        }
    }
    private static BlockSyntax CreateInvokeWithoutMarshalling(IMethodSymbol symbol, 
        List<(IParameterSymbol parameter, IMarshaller marshaller)> parameters)
    {
        ExpressionSyntax pInvokeExpression = InvocationExpression(IdentifierName("__PInvoke"))
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(Argument(IdentifierName("_handle")))
                        .AddRange(
                            parameters.Select(x => GetArgumentForParameter(x.parameter, x.marshaller)
                            )
                        )
                )
            );

        // No marshalling required, call __PInvoke and return
        if (symbol.ReturnsVoid)
        {
            return Block(ExpressionStatement(pInvokeExpression));
        }
            
        if (symbol.ReturnsByRef || symbol.ReturnsByRefReadonly)
        {
            pInvokeExpression = RefExpression(pInvokeExpression);
        }

        return Block(ReturnStatement(pInvokeExpression));
    }

    private static BlockSyntax CreateInvokeWithMarshalling(IMethodSymbol symbol,
        List<(IParameterSymbol parameter, IMarshaller marshaller)> parameters)
    {
        var returnMarshaller = GetMarshaller(symbol.ReturnType);

        ExpressionSyntax pInvokeExpression = InvocationExpression(IdentifierName("__PInvoke"))
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(Argument(IdentifierName("_handle")))
                        .AddRange(
                            parameters.Select(x => GetArgumentForParameter(x.parameter, x.marshaller)
                            )
                        )
                )
            );
        
        if (!symbol.ReturnsVoid)
        {
            // TODO: ref return
            pInvokeExpression = AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName("__retVal"), pInvokeExpression);
        }

        // The generated method consists of the following content:
        //
        // locals: Generate locals for marshalled types and return value
        // setup: Let marshallers setup
        // try
        // {
        //   marshal: managed to native
        //   {
        //     pinned_marshal: bring unmanaged into scope
        //     p/invoke 
        //   }
        //   unmarshal: native to managed
        // }
        // finally
        // {
        //   cleanup: let marshallers free memory
        // }
        //
        // return: retval

        SyntaxList<TNode> Step<TNode>(string comment, Func<IParameterSymbol, IMarshaller, SyntaxList<TNode>> marshaller, SyntaxList<TNode> additional = default) where TNode : SyntaxNode
        {
            var result = List(parameters.Where(x => x.marshaller != null)
                .SelectMany(x => marshaller(x.parameter, x.marshaller)));

            result = result.AddRange(additional);

            if (comment != null && result.Count > 0)
            {
                result = result.Replace(
                    result[0],
                    result[0].WithLeadingTrivia(Comment(comment)));
            }
            return result;
        }

        var statements = List<StatementSyntax>();
        var tryStatements = List<StatementSyntax>();
        var finallyStatements = List<StatementSyntax>();

        // locals
        statements = statements.AddRange(
            Step(null, (p, m) => 
                SingletonList<StatementSyntax>(
                    CreateLocalDeclarationWithDefaultValue(m.ToMarshalledType(p.Type), $"__{p.Name}_native"))));

        if (!symbol.ReturnsVoid)
        {
            var returnType = ToTypeSyntax(symbol.ReturnType);
            statements = statements.Add(CreateLocalDeclarationWithDefaultValue(returnType, "__retVal"));

        }

        if (returnMarshaller != null)
        {
            var nativeType = returnMarshaller.ToMarshalledType(symbol.ReturnType);
            statements = statements.Add(CreateLocalDeclarationWithDefaultValue(nativeType, "__retVal_native"));
        }

        // setup
        var setup = Step("// Setup", (p, m) => m.Setup(p));
        statements = statements.AddRange(setup);

        // TODO @ all steps; check if is ref/out/etc to know if step is required

        // marshal
        var marshal = Step("// Marshal - Convert managed data to native data.", (p, m) => m.Marshal(p));
        tryStatements = tryStatements.AddRange(marshal);

        // PinnedMarshal
        var pinnedMarshal = Step("// PinnedMarshal", (p, m) => m.PinnedMarshal(p));

        tryStatements = tryStatements
            .Add(
                Block(
                    pinnedMarshal.Add(
                        ExpressionStatement(pInvokeExpression))));

        // Unmarshal
        var unmarshal = Step("// Unmarshal - Convert native data to managed data.", (p, m) => m.Unmarshal(p),
            returnMarshaller?.Unmarshal(null) ?? default);

        tryStatements = tryStatements.AddRange(unmarshal);

        // Cleanup
        var cleanup = Step("// Cleanup - Perform cleanup of caller allocated resources.", (p, m) => m.Cleanup(p));
        finallyStatements = finallyStatements.AddRange(cleanup);

        // try / finally
        statements = statements.Add(TryStatement()
            .WithBlock(Block(tryStatements))
            .WithFinally(
                FinallyClause(
                    Block(finallyStatements)))
        );

        if (!symbol.ReturnsVoid)
        {
            statements = statements.Add(
                ReturnStatement(
                    IdentifierName("__retVal")));
        }

        return Block(statements);
    }
    
    private static LocalDeclarationStatementSyntax CreateLocalDeclarationWithDefaultValue(TypeSyntax type, string identifier) =>
        LocalDeclarationStatement(
            VariableDeclaration(type,
                SingletonSeparatedList(
                    VariableDeclarator(Identifier(identifier))
                        .WithInitializer(
                            EqualsValueClause(
                                LiteralExpression(SyntaxKind.DefaultLiteralExpression, Token(SyntaxKind.DefaultKeyword)))))));

    private static LocalFunctionStatementSyntax CreateExternFunction(IMethodSymbol symbol, 
        TypeSyntax externReturnType, 
        IEnumerable<(IParameterSymbol parameter, IMarshaller marshaller)> parameterMarshallers)
    {
        var cdecl = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression, 
            ParseTypeName("System.Runtime.InteropServices.CallingConvention"), 
            IdentifierName("Cdecl"));
        
        var handleParam = Parameter(Identifier("handle_")).WithType(ParseTypeName("nint"));

        var externParameters = ToParameterListSyntax(handleParam, parameterMarshallers);

        return LocalFunctionStatement(externReturnType, "__PInvoke")
            .WithModifiers(TokenList(          
                Token(SyntaxKind.StaticKeyword),
                Token(SyntaxKind.UnsafeKeyword),
                Token(SyntaxKind.ExternKeyword)
                ))
            .WithParameterList(externParameters)
            .WithAttributeLists(
                SingletonList(
                    AttributeList(
                        SeparatedList(new[]
                        {
                            Attribute(ParseName("global::System.Runtime.InteropServices.DllImportAttribute"),
                                AttributeArgumentList(
                                    SeparatedList(new[] {
                                        AttributeArgument(
                                            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("SampSharp"))
                                        ),
                                        AttributeArgument(cdecl)
                                            .WithNameEquals(NameEquals("CallingConvention")),
                                        AttributeArgument(
                                                LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(ToExternName(symbol)))
                                            )
                                            .WithNameEquals(NameEquals("EntryPoint")),
                                        AttributeArgument(
                                                LiteralExpression(SyntaxKind.TrueLiteralExpression)
                                            )
                                            .WithNameEquals(NameEquals("ExactSpelling"))
                                    })
                                )
                            )
                        })
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
            .WithLeadingTrivia(Comment("// Local P/Invoke"));
    }

    private static string ToExternName(IMethodSymbol symbol)
    {
        var type = symbol.ContainingType;

        var overload = symbol.GetAttributes(Constants.OverloadAttributeFQN).FirstOrDefault()?.ConstructorArguments[0].Value as string;
        var functionName = symbol.GetAttributes(Constants.FunctionAttributeFQN).FirstOrDefault()?.ConstructorArguments[0].Value as string;

        if (functionName != null)
        {
            return $"{type.Name}_{functionName}";
        }

        return $"{type.Name}_{FirstLower(symbol.Name)}{overload}";
    }
    
    private static string FirstLower(string value)
    {
        return $"{char.ToLowerInvariant(value[0])}{value.Substring(1)}";
    }

    private static IMarshaller GetMarshaller(ITypeSymbol typeSyntax)
    {
        switch (typeSyntax.SpecialType)
        {
            case SpecialType.System_Boolean:
                return BooleanMarshalling.Instance;
            case SpecialType.System_String:
                return StringMarshalling.Instance;
        }

        if (typeSyntax.SpecialType != SpecialType.None)
        {
            return null;
        }

        // TODO: check for type marshalling attributes
        return null;
    }
    
    private static IMarshaller GetParameterMarshaller(IParameterSymbol parameterSymbol)
    {
        return GetMarshaller(parameterSymbol.Type);
    }
    
    private static ParameterListSyntax ToParameterListSyntax(ParameterSyntax first, IEnumerable<(IParameterSymbol symbol, IMarshaller marshaller)> parameters)
    {
        return ParameterList(
            SingletonSeparatedList(first)
                .AddRange(
                    parameters
                        .Select(parameter => Parameter(Identifier(parameter.symbol.Name))
                        .WithType(parameter.marshaller?.ToMarshalledType(parameter.symbol.Type) ?? ToTypeSyntax(parameter.symbol.Type))
                        .WithModifiers(GetRefTokens(parameter.symbol.RefKind)))));
    }

    private static SyntaxTokenList GetRefTokens(RefKind refKind)
    {
        return refKind switch
        {
            RefKind.Ref => TokenList(Token(SyntaxKind.RefKeyword)),
            RefKind.Out => TokenList(Token(SyntaxKind.OutKeyword)),
            _ => default
        };
    }
    
    private static TypeSyntax ToReturnTypeSyntax(IMethodSymbol symbol)
    {
        return ToTypeSyntax(symbol.ReturnType, symbol.ReturnsByRef || symbol.ReturnsByRefReadonly);
    }

    public static TypeSyntax ToTypeSyntax(ITypeSymbol symbol, bool isRef = false, bool isReadonly = false)
    {
        var result = ParseTypeName(symbol.SpecialType == SpecialType.None 
            ? $"global::{symbol.ToDisplayString()}" 
            : symbol.ToDisplayString());

        if (isRef)
        {
            result = isReadonly 
                ? RefType(result).WithReadOnlyKeyword(Token(SyntaxKind.ReadOnlyKeyword)) 
                : RefType(result);
        }
        return result;
    }

    private static SeparatedSyntaxList<ExpressionSyntax> CreateList(params ExpressionSyntax[] values)
    {
        return new SeparatedSyntaxList<ExpressionSyntax>().AddRange(values);
    }

    private static ParameterListSyntax ToParameterListSyntax(ImmutableArray<IParameterSymbol> parameters)
    {
        return ParameterList(
            SeparatedList(
                parameters.Select(x => 
                    Parameter(Identifier(x.Name))
                        .WithType(ToTypeSyntax(x.Type))
                        .WithModifiers(GetRefTokens(x.RefKind)))
            )
        );
    }

    private static StructDeclaration GetStructDeclaration(GeneratorAttributeSyntaxContext ctx, CancellationToken cancellationToken)
    {
        var targetNode = (StructDeclarationSyntax)ctx.TargetNode;
        if (ctx.TargetSymbol is not INamedTypeSymbol symbol)
            return null;
        
        // partial, non-static, non-generic
        List<(MethodDeclarationSyntax, IMethodSymbol)> methods = targetNode.Members.OfType<MethodDeclarationSyntax>()
            .Where(x => x.IsPartial() && !x.HasModifier(SyntaxKind.StaticKeyword) && x.TypeParameterList == null)
            .Select(methodDeclaration => ctx.SemanticModel.GetDeclaredSymbol(methodDeclaration, cancellationToken) is not { } methodSymbol 
                ? (null, null)
                : (methodDeclaration, methodSymbol))
            .Where(x => x.methodSymbol != null)
            .ToList();

        return new StructDeclaration(symbol, targetNode, methods);
    }

    private record StructDeclaration(
        ISymbol Symbol,
        StructDeclarationSyntax Node,
        List<(MethodDeclarationSyntax node, IMethodSymbol symbol)> Methods);
}
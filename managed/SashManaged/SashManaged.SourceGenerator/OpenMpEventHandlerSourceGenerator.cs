﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Marshalling;
using SashManaged.SourceGenerator.SyntaxFactories;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.HelperSyntaxFactory;

namespace SashManaged.SourceGenerator;

/// <summary>
/// This source generator generates event handler interfaces for open.mp events. The generated interface contains a
/// default implementation for IncreaseReference/DecreaseReference methods, which are used to creating an unmanaged
/// event handler for the managed event handler. The implementation invokes the native function
/// `{EventHandlerName}Impl_create`/_delete to create the unmanaged event handler. The create call will include native
/// handles of delegate functions for every event method in the interface.
/// </summary>
[Generator]
public class OpenMpEventHandlerSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var attributedInterfaces = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                Constants.EventHandlerAttributeFQN, 
                static (s, _) => s is InterfaceDeclarationSyntax str && str.IsPartial(), 
                static(ctx, ct) => GetInterfaceDeclaration(ctx, ct))
            .Where(x => x is not null);

        context.RegisterSourceOutput(attributedInterfaces, (ctx, info) =>
        {
            var interfaceDeclaration = InterfaceDeclaration(info!.Syntax.Identifier)
                .WithModifiers(info.Syntax.Modifiers)
                .WithMembers(GenerateInterfaceMembers(info))
                .WithBaseList(
                    BaseList(SingletonSeparatedList<BaseTypeSyntax>(
                            SimpleBaseType(
                                IdentifierNameGlobal(Constants.EventHandlerFQN)))));
            
            var unit = CompilationUnit()
                .AddMembers(NamespaceDeclaration(ParseName(info.Symbol.ContainingNamespace.ToDisplayString()))
                    .AddMembers(interfaceDeclaration))
                .WithLeadingTrivia(
                    TriviaFactory.AutoGeneratedComment());

            var sourceText = unit.NormalizeWhitespace()
                .GetText(Encoding.UTF8);

            ctx.AddSource($"{info.Symbol.Name}.g.cs", sourceText);
        });
    }

    private static SyntaxList<MemberDeclarationSyntax> GenerateInterfaceMembers(EventInterfaceStubGenerationContext ctx)
    {
        // TODO: marshalling

        var delegateVars = ctx.Methods.Select(method =>
            VariableDeclarator(
                    Identifier($"__{method.Symbol.Name}_delegate"))
                .WithInitializer(
                    EqualsValueClause(
                        CastExpression(
                            IdentifierName($"{method.Symbol.Name}_"),
                            IdentifierName(method.Symbol.Name)))));

        var functionPointerVars = ctx.Methods.Select(method => 
            VariableDeclarator(
                    Identifier($"__{method.Symbol.Name}_ptr"))
                .WithInitializer(
                    EqualsValueClause(
                        InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierNameGlobal(Constants.MarshalFQN),
                                    IdentifierName(nameof(Marshal.GetFunctionPointerForDelegate))))
                            .WithArgumentList(
                                ArgumentList(
                                    SingletonSeparatedList(
                                        Argument(
                                            IdentifierName($"__{method.Symbol.Name}_delegate"))))))));

        var functionPointerArgs = ctx.Methods.Select(method =>
            Argument(IdentifierName($"__{method.Symbol.Name}_ptr")));

        var delegateArgs = ctx.Methods.Select(method => Argument(IdentifierName($"__{method.Symbol.Name}_delegate")));

        var increateReferenceMember = MethodDeclaration(
                IdentifierName("nint"),
                Identifier("IncreaseReference"))
            .WithExplicitInterfaceSpecifier(
                ExplicitInterfaceSpecifier(
                    IdentifierNameGlobal(Constants.EventHandlerFQN)))
            .WithBody(
                Block(
                    LocalDeclarationStatement(
                        VariableDeclaration(
                                NullableType(
                                    IdentifierName("nint")))
                            .WithVariables(
                                SingletonSeparatedList(
                                    VariableDeclarator(
                                            Identifier("__existingHandle"))
                                        .WithInitializer(
                                            EqualsValueClause(
                                                InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierNameGlobal(Constants.EventHandlerNativeHandleStorageFQN),
                                                            IdentifierName("GetAndIncreaseReference")))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SingletonSeparatedList(
                                                                Argument(
                                                                    ThisExpression()))))))))),
                    IfStatement(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("__existingHandle"),
                            IdentifierName("HasValue")),
                        Block(SingletonList<StatementSyntax>(
                                ReturnStatement(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("__existingHandle"),
                                        IdentifierName("Value")))))),
                    LocalDeclarationStatement(
                        VariableDeclaration(
                                IdentifierName(
                                    IdentifierGlobal(Constants.DelegateFQN)))
                            .WithVariables(
                                SeparatedList(delegateVars))),
                    LocalDeclarationStatement(
                        VariableDeclaration(
                                IdentifierName("nint"))
                            .WithVariables(
                                SeparatedList(functionPointerVars))),
                    LocalDeclarationStatement(
                        VariableDeclaration(
                                IdentifierName("nint"))
                            .WithVariables(
                                SingletonSeparatedList(
                                    VariableDeclarator(
                                            Identifier("handle"))
                                        .WithInitializer(
                                            EqualsValueClause(
                                                InvocationExpression(
                                                        IdentifierName("__PInvoke"))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SeparatedList(functionPointerArgs)))))))),
                    ExpressionStatement(
                        InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierNameGlobal(Constants.EventHandlerNativeHandleStorageFQN),
                                    IdentifierName("CreateReference")))
                            .WithArgumentList(
                                ArgumentList(
                                    SeparatedList([
                                            Argument(
                                                ThisExpression()),
                                            Argument(
                                                IdentifierName("handle"))
                                        ])
                                        .AddRange(delegateArgs)))),
                    ReturnStatement(
                        IdentifierName("handle")),
                    GenerateExternFunctionCreate(ctx)));


        var decreaseReferenceMember = MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.VoidKeyword)),
                Identifier("DecreaseReference"))
            .WithExplicitInterfaceSpecifier(
                ExplicitInterfaceSpecifier(
                    IdentifierNameGlobal(Constants.EventHandlerFQN)))
            .WithBody(
                Block(
                    LocalDeclarationStatement(
                        VariableDeclaration(
                                NullableType(
                                    IdentifierName("nint")))
                            .WithVariables(
                                SingletonSeparatedList(
                                    VariableDeclarator(
                                            Identifier("handleToRemove"))
                                        .WithInitializer(
                                            EqualsValueClause(
                                                InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierNameGlobal(Constants.EventHandlerNativeHandleStorageFQN),
                                                            IdentifierName("GetAndDecreaseReference")))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SingletonSeparatedList(
                                                                Argument(
                                                                    ThisExpression()))))))))),
                    IfStatement(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("handleToRemove"),
                            IdentifierName("HasValue")),
                        Block(SingletonList<StatementSyntax>(
                                ExpressionStatement(
                                    InvocationExpression(
                                            IdentifierName("__PInvoke"))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList(
                                                    Argument(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName("handleToRemove"),
                                                            IdentifierName("Value")))))))))),
                    GenerateExternFunctionDelete(ctx)
                ));

        var delegates = ctx.Methods.Select(method =>
        {
            var parameters = ToParameterListSyntax([], method.Parameters.Select(x => ToForwardInfo(x.Symbol, x.MarshallerShape, false)));
            var returnType = method.ReturnMarshallerShape?.GetNativeType() ?? 
                             TypeNameGlobal(method.Symbol.ReturnType);

            return DelegateDeclaration(
                    returnType,
                    Identifier($"{method.Symbol.Name}_"))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PrivateKeyword)))
                .WithParameterList(parameters);
        });
        
        return List<MemberDeclarationSyntax>(delegates)
            .AddRange([
                increateReferenceMember, 
                decreaseReferenceMember
            ]);
    }
    
    private static LocalFunctionStatementSyntax GenerateExternFunctionCreate(EventInterfaceStubGenerationContext ctx)
    {
        var parameters = ctx.Methods.Select(x => new ParamForwardInfo($"_{x.Symbol.Name}", ParseTypeName("nint"), RefKind.In));

        return GenerateExternFunction(
            library: ctx.Library, 
            externName: $"{ctx.NativeTypeName}Impl_create",
            externReturnType: ParseTypeName("nint"), 
            parameters: parameters);
    }
    
    private static LocalFunctionStatementSyntax GenerateExternFunctionDelete(EventInterfaceStubGenerationContext ctx)
    {
        var parameters = new[] { new ParamForwardInfo("ptr", ParseTypeName("nint"), RefKind.In) };

        return GenerateExternFunction(
            library: ctx.Library, 
            externName: $"{ctx.NativeTypeName}Impl_delete",
            externReturnType: PredefinedType(Token(SyntaxKind.VoidKeyword)), 
            parameters: parameters);
    }

    /// <summary>
    /// Returns a context for the generation of the interface declaration.
    /// </summary>
    private static EventInterfaceStubGenerationContext? GetInterfaceDeclaration(GeneratorAttributeSyntaxContext ctx, CancellationToken cancellationToken)
    {
        var targetNode = (InterfaceDeclarationSyntax)ctx.TargetNode;
        if (ctx.TargetSymbol is not INamedTypeSymbol symbol)
            return null;

        var attribute = ctx.Attributes.Single();

        var library = attribute.NamedArguments.FirstOrDefault(x => x.Key == "Library")
            .Value.Value as string ?? "SampSharp";

        var nativeTypeName = attribute.NamedArguments.FirstOrDefault(x => x.Key == "NativeTypeName")
            .Value.Value as string ?? (symbol.Name.StartsWith("I") ? symbol.Name.Substring(1) : symbol.Name);

        var wellKnownMarshallerTypes = MarshallingCodeGenerator.GetWellKnownMarshallerTypes(ctx.SemanticModel.Compilation);

        // filter methods: non-static
        var methods = targetNode.Members.OfType<MethodDeclarationSyntax>()
            .Where(x => !x.HasModifier(SyntaxKind.StaticKeyword))
            .Select(methodDeclaration => ctx.SemanticModel.GetDeclaredSymbol(methodDeclaration, cancellationToken) is not { } methodSymbol
                ? (null, null)
                : (methodDeclaration, methodSymbol))
            .Where(x => x.methodSymbol != null)
            .Select(method =>
            {
                var parameters = method.methodSymbol!.Parameters.Select(parameter =>
                        new ParameterStubGenerationContext(parameter, MarshallerShapeFactory.GetMarshallerShape(parameter, wellKnownMarshallerTypes)))
                    .ToArray();

                var returnMarshallerShape = MarshallerShapeFactory.GetMarshallerShape(method.methodSymbol, wellKnownMarshallerTypes);
                var requiresMarshalling = returnMarshallerShape != null || parameters.Any(x => x.MarshallerShape != null);

                if (returnMarshallerShape != null && (method.methodSymbol.ReturnsByRef || method.methodSymbol.ReturnsByRefReadonly))
                {
                    // marshalling return-by-ref not supported.
                    // TODO: diagnostic
                    return null;
                }

                return new EventMethodStubGenerationContext(method.methodDeclaration!, method.methodSymbol, parameters, returnMarshallerShape, requiresMarshalling);
            })
            .Where(x => x != null)
            .ToArray();

        return new EventInterfaceStubGenerationContext(symbol, targetNode, methods!, library, nativeTypeName);
    }
}
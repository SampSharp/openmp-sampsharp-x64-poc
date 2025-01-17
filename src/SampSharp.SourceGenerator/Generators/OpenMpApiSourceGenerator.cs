﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Generators.ApiStructs;
using SampSharp.SourceGenerator.Helpers;
using SampSharp.SourceGenerator.Marshalling;
using SampSharp.SourceGenerator.Models;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators;

/// <summary>
/// This source generator generates the marshalling interop methods for open.mp API structs. The generator generates the
/// following:
/// <list type="bullet">
///     <item>Implementation of the IPointer interface</item>
///     <item>Implementation of the IEquatable{self} interface</item>
///     <item>P/Invoke every partial method in the interface with marshalling of every parameter and return value</item>
///     <item>For every "implementing" interface specified in the CodeGen attribute generate the following:
///        <list type="bullet">
///             <item>Implementation of the IEquatable{other} interface</item>
///             <item>Pass-through implementations of all methods in the implementing interface</item>
///         </list>
///     </item>
/// </list>
/// </summary>
[Generator]
public class OpenMpApiSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Debugger.Launch();

        var attributedStructs = context.SyntaxProvider.ForAttributeWithMetadataName(
                Constants.ApiAttributeFQN,
                static (s, _) => s is StructDeclarationSyntax str && str.IsPartial(), 
                static (ctx, ct) => GetStructDeclaration(ctx, ct))
            .Where(x => x is not null);

        context.RegisterSourceOutput(attributedStructs, (ctx, info) =>
        {
            var unit = GenerateUnit(info);

            var sourceText = unit.NormalizeWhitespace(elasticTrivia: true)
                .GetText(Encoding.UTF8);

            ctx.AddSource($"{info!.Symbol.Name}.g.cs", sourceText);
        });
    }

    private static CompilationUnitSyntax GenerateUnit(StructStubGenerationContext? info)
    {
        var modifiers = info!.Syntax.Modifiers;
            
        if (!info.Syntax.HasModifier(SyntaxKind.UnsafeKeyword))
        {
            modifiers = modifiers.Insert(modifiers.IndexOf(SyntaxKind.PartialKeyword), Token(SyntaxKind.UnsafeKeyword));
        }
        
        var structDeclaration = StructDeclaration(info.Syntax.Identifier)
            .WithModifiers(modifiers)
            .WithMembers(GenerateStructMembers(info))
            .WithBaseList(
                BaseList(
                    SeparatedList<BaseTypeSyntax>(
                        GenerateBaseTypes(info))))
            .WithAttributeLists(
                List([
                    AttributeFactory.GeneratedCode(),
                    AttributeFactory.SkipLocalsInit()
                ]));

        var namespaceDeclaration = NamespaceDeclaration(
                ParseName(info.Symbol.ContainingNamespace.ToDisplayString()))
            .AddMembers(structDeclaration);

        var unit = CompilationUnit()
            .AddMembers(namespaceDeclaration)
            .WithLeadingTrivia(
                TriviaFactory.AutoGeneratedComment());
        return unit;
    }

    /// <summary>
    /// Returns the base types for the generated struct implementation.
    /// </summary>
    private static IEnumerable<SimpleBaseTypeSyntax> GenerateBaseTypes(StructStubGenerationContext ctx)
    {
        var result = ctx.ImplementingTypes.Select(x => 
                SimpleBaseType(
                    GenericType(Constants.IEquatableFQN, TypeNameGlobal(x))))
            .Concat([
                SimpleBaseType(
                        GenericType(Constants.IEquatableFQN, ParseTypeName(ctx.Symbol.Name))),
                SimpleBaseType(
                    TypeNameGlobal(Constants.PointerFQN))
            ]);
        
        if (ctx.IsComponent)
        {
            result = result.Append(
                SimpleBaseType(
                    GenericType(Constants.ComponentInterfaceFQN, ParseTypeName(ctx.Symbol.Name))));
        }

        if (ctx.IsExtension)
        {
            result = result.Append(
                SimpleBaseType(
                    GenericType(Constants.ExtensionInterfaceFQN, ParseTypeName(ctx.Symbol.Name))));
        }

        return result;
    }

    /// <summary>
    /// Returns the members for the struct implementation.
    /// </summary>
    private static SyntaxList<MemberDeclarationSyntax> GenerateStructMembers(StructStubGenerationContext ctx)
    {
        return List([
            ..CreationMembersGenerator.GenerateCreationMembers(ctx), 
            ..EqualityMembersGenerator.GenerateEqualityMembers(ctx),
            ..ForwardingMembersGenerator.GenerateImplementingTypeMembers(ctx),
            ..NativeMembersGenerator.GenerateNativeMethods(ctx)
        ]);
    }
    
    /// <summary>
    /// Returns the struct generation context for a code gen context.
    /// </summary>
    private static StructStubGenerationContext? GetStructDeclaration(GeneratorAttributeSyntaxContext ctx, CancellationToken cancellationToken)
    {
        var targetNode = (StructDeclarationSyntax)ctx.TargetNode;
        if (ctx.TargetSymbol is not INamedTypeSymbol symbol)
        {
            return null;
        }

        var attribute = ctx.Attributes.Single();

        var library = attribute.NamedArguments.FirstOrDefault(x => x.Key == "Library")
            .Value.Value as string ?? "SampSharp";

        var nativeTypeName = attribute.NamedArguments.FirstOrDefault(x => x.Key == "NativeTypeName")
            .Value.Value as string ?? symbol.Name;

        var wellKnownMarshallerTypes = WellKnownMarshallerTypes.Create(ctx.SemanticModel.Compilation);
        var ctxFactory = new IdentifierStubContextFactory(wellKnownMarshallerTypes);


        // TODO implementingTypes for inheritance with depth > 1
        var implementingTypes = attribute.ConstructorArguments[0]
            .Values.Select(x => (ITypeSymbol)x.Value!)
            .ToArray();

        var isComponent = implementingTypes.Any(x => x.ToDisplayString() == Constants.ComponentFQN);
        var isExtension = implementingTypes.Any(x => x.ToDisplayString() == Constants.ExtensionFQN);

        // filter methods: partial, non-static, non-generic
        var methods = targetNode.Members
            .OfType<MethodDeclarationSyntax>()
            .Where(x => x.IsPartial() && !x.HasModifier(SyntaxKind.StaticKeyword) && x.TypeParameterList == null)
            .Select(methodDeclaration => ctx.SemanticModel.GetDeclaredSymbol(methodDeclaration, cancellationToken) is { } methodSymbol
                ? (methodDeclaration, methodSymbol)
                : (null, null))
            .Where(x => x.methodSymbol != null)
            .Select(method =>
            {
                
                var parameters = method.methodSymbol!.Parameters.Select(parameter => ctxFactory.Create(parameter, MarshalDirection.ManagedToUnmanaged))
                    .ToArray();
                
                var returnValueContext = ctxFactory.Create(method.methodSymbol, MarshalDirection.ManagedToUnmanaged);

                var requiresMarshalling = returnValueContext.Shape != MarshallerShape.None || parameters.Any(x => x.Shape != MarshallerShape.None);

                if (returnValueContext.Shape != MarshallerShape.None && (method.methodSymbol.ReturnsByRef || method.methodSymbol.ReturnsByRefReadonly))
                {
                    // marshalling return-by-ref not supported.
                    // TODO: diagnostic
                    return null;
                }


                return new ApiMethodStubGenerationContext(
                    method.methodDeclaration!,
                    method.methodSymbol, 
                    parameters, 
                    returnValueContext,
                    requiresMarshalling, 
                    library,
                    nativeTypeName);
            })
            .Where(x => x != null)
            .ToArray();

        return new StructStubGenerationContext(symbol, targetNode, methods!, implementingTypes, isExtension, isComponent);
    }
}
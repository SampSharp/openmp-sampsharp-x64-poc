using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Helpers;
using SashManaged.SourceGenerator.Marshalling;
using SashManaged.SourceGenerator.Marshalling.Shapes;
using SashManaged.SourceGenerator.Models;
using SashManaged.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.HelperSyntaxFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.StatementFactory;
using static SashManaged.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SashManaged.SourceGenerator.Generators.ApiStructs;

public static class NativeMembersGenerator
{
    public static IEnumerable<MemberDeclarationSyntax> GenerateNativeMethods(StructStubGenerationContext ctx)
    {
        return ctx.Methods
            .Select(GenerateNativeMethod)
            .Where(x => x != null);
    }

    /// <summary>
    /// Returns a method declaration for a native method including marshalling of parameters and return value.
    /// </summary>
    private static MemberDeclarationSyntax GenerateNativeMethod(MethodStubGenerationContext ctx)
    {
        var invocation = CreateInvocation(ctx);
        
        // Extern P/Invoke
        var externReturnType = ctx.ReturnMarshallerShape?.GetNativeType() ?? 
                               TypeNameGlobal(ctx.Symbol.ReturnType);

        if(ctx.ReturnsByRef)
        {
            externReturnType = ctx.RequiresMarshalling
                ? PointerType(externReturnType)
                : RefType(externReturnType);
        }

        var externFunction = GenerateExternFunction(ctx, externReturnType);

        invocation = invocation.WithStatements(invocation.Statements.Add(externFunction));
     
        return MethodDeclaration(TypeNameGlobal(ctx.Symbol), ctx.Declaration.Identifier)
            .WithModifiers(ctx.Declaration.Modifiers)
            .WithParameterList(ToParameterListSyntax(ctx.Symbol.Parameters, false))
            .WithBody(invocation);
    }

    /// <summary>
    /// Returns a block with the invocation of the native method including marshalling of parameters and return value.
    /// </summary>
    private static BlockSyntax CreateInvocation(MethodStubGenerationContext ctx)
    {
        return ctx.RequiresMarshalling 
            ? CreateInvocationWithMarshalling(ctx)
            : CreateInvocationWithoutMarshalling(ctx);
    }

    private static BlockSyntax CreateInvocationWithoutMarshalling(MethodStubGenerationContext ctx)
    {
        ExpressionSyntax invoke = InvocationExpression(IdentifierName("__PInvoke"))
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(Argument(IdentifierName("_handle")))
                        .AddRange(
                            ctx.Parameters.Select(GetArgumentForPInvokeParameter)
                        )
                )
            );

        if (ctx.Symbol.ReturnsVoid)
        {
            return Block(ExpressionStatement(invoke));
        }
            
        if (ctx.Symbol.ReturnsByRef || ctx.Symbol.ReturnsByRefReadonly)
        {
            invoke = RefExpression(invoke);
        }

        return Block(ReturnStatement(invoke));
    }

    private static BlockSyntax CreateInvocationWithMarshalling(MethodStubGenerationContext ctx)
    {
        ExpressionSyntax invoke = 
            InvocationExpression(IdentifierName("__PInvoke"))
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                                Argument(IdentifierName("_handle")))
                            .AddRange(
                                ctx.Parameters.Select(GetArgumentForPInvokeParameter))));
        
        if (!ctx.Symbol.ReturnsVoid)
        {
            invoke = 
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression, 
                    IdentifierName(ctx.ReturnMarshallerShape == null ? "__retVal" : "__retVal_native"), 
                    invoke);
        }

        // The generated method consists of the following content:
        //
        // LocalsInit - Generate locals for marshalled types and return value.
        // Setup - Perform required setup.
        // try
        // {
        //   Marshal - Convert managed data to native data.
        //   {
        //     PinnedMarshal - Convert managed data to native data that requires the managed data to be pinned.
        //     p/invoke 
        //   }
        //   [[__invokeSucceeded = true;]]
        //   NotifyForSuccessfulInvoke - Keep alive any managed objects that need to stay alive across the call.
        //   UnmarshalCapture - Capture the native data into marshaller instances in case conversion to managed data throws an exception.
        //   Unmarshal - Convert native data to managed data.
        // }
        // finally
        // {
        //   if (__invokeSucceeded)
        //   {
        //      GuaranteedUnmarshal - Convert native data to managed data even in the case of an exception during the non-cleanup phases.
        //      CleanupCalleeAllocated - Perform cleanup of callee allocated resources.
        //   }
        //   CleanupCallerAllocated - Perform cleanup of caller allocated resources.
        // }
        //
        // return: retVal
        //
        // NOTES:
        // - design doc: https://github.com/dotnet/runtime/blob/main/docs/design/libraries/LibraryImportGenerator/UserTypeMarshallingV2.md
        // - we're supporting Default, ManagedToUnmanagedIn, ManagedToUnmanagedOut, ManagedToUnmanagedRef
        // - not implementing element marshalling (arrays) at the moment.

        // collect all marshalling steps
        var setup = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_SETUP, (p, m) => m.Setup(p), ctx.ReturnMarshallerShape?.Setup(null) ?? default);
        var marshal = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_MARSHAL, (p, m) => m.Marshal(p), ctx.ReturnMarshallerShape?.Marshal(null) ?? default);
        var pinnedMarshal = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_PINNED_MARSHAL, (p, m) => m.PinnedMarshal(p), ctx.ReturnMarshallerShape?.PinnedMarshal(null) ?? default);
        var pin = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_PIN, (p, m) => m.Pin(p));
        var notify = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_NOTIFY, (p, m) => m.NotifyForSuccessfulInvoke(p), ctx.ReturnMarshallerShape?.NotifyForSuccessfulInvoke(null) ?? default);
        var unmarshalCapture = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_UNMARSHAL_CAPTURE, (p, m) => m.UnmarshalCapture(p), ctx.ReturnMarshallerShape?.UnmarshalCapture(null) ?? default);
        var unmarshal = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_UNMARSHAL, (p, m) => m.Unmarshal(p), ctx.ReturnMarshallerShape?.Unmarshal(null) ?? default);
        var guaranteedUnmarshal = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_GUARANTEED_UNMARSHAL, (p, m) => m.GuaranteedUnmarshal(p), ctx.ReturnMarshallerShape?.GuaranteedUnmarshal(null) ?? default);
        var cleanupCallee = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_CLEANUP_CALLEE, (p, m) => m.CleanupCalleeAllocated(p), ctx.ReturnMarshallerShape?.CleanupCalleeAllocated(null) ?? default);
        var cleanupCaller = Step(ctx, MarshallingCodeGenDocumentation.COMMENT_CLEANUP_CALLER, (p, m) => m.CleanupCallerAllocated(p), ctx.ReturnMarshallerShape?.CleanupCallerAllocated(null) ?? default);
        
        // init locals
        var statements = Step(ctx, null, (p, m) =>
        {
            if (!m.RequiresLocal)
            {
                return new SyntaxList<StatementSyntax>();
            }

            var identifier = $"__{p.Name}_native";

            return SingletonList<StatementSyntax>(CreateLocalDeclarationWithDefaultValue(m.GetNativeType(), identifier));
        });

        if (!ctx.Symbol.ReturnsVoid)
        {
            var returnType = TypeNameGlobal(ctx.Symbol.ReturnType);
            
            if (ctx.ReturnsByRef)
            {
                returnType = PointerType(returnType);
            }

            statements = statements.Add(CreateLocalDeclarationWithDefaultValue(returnType, "__retVal"));
            
            if (ctx.ReturnMarshallerShape != null)
            {
                var nativeType = ctx.ReturnMarshallerShape.GetNativeType();
                statements = statements.Add(CreateLocalDeclarationWithDefaultValue(nativeType, "__retVal_native"));
            }
        }

        // if callee cleanup is required, we need to keep track of invocation success
        if (cleanupCallee.Count > 0 || guaranteedUnmarshal.Count > 0)
        {
            statements = statements.Add(
                CreateLocalDeclarationWithDefaultValue(PredefinedType(Token(SyntaxKind.BoolKeyword)), "__invokeSucceeded"));
        
            notify = notify.Insert(0, 
                ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression, 
                        IdentifierName("__invokeSucceeded"), 
                        LiteralExpression(SyntaxKind.TrueLiteralExpression))));
        
            cleanupCallee = SingletonList<StatementSyntax>(
                        IfStatement(IdentifierName("__invokeSucceeded"), 
                        Block(guaranteedUnmarshal.AddRange(cleanupCallee))));
        }
        
        // wire up steps
        var finallyStatements = cleanupCallee.AddRange(cleanupCaller);

        StatementSyntax pinnedBlock = Block(pinnedMarshal.Add(ExpressionStatement(invoke)));

        for (var i = pin.Count - 1; i >= 0; i--)
        {
            pinnedBlock = pin[i].WithStatement(pinnedBlock);
        }

        var guarded = marshal
            .Add(pinnedBlock)
            .AddRange(notify)
            .AddRange(unmarshalCapture)
            .AddRange(unmarshal);
        
        statements = statements.AddRange(setup);

        if (finallyStatements.Any())
        {
            statements = statements.Add(TryStatement()
                .WithBlock(Block(guarded))
                .WithFinally(
                    FinallyClause(
                        Block(finallyStatements)))
            );
        }
        else
        {
            statements = statements.AddRange(guarded);
        }

        if (!ctx.Symbol.ReturnsVoid)
        {
            ExpressionSyntax returnExpression = IdentifierName("__retVal");

            if (ctx.ReturnsByRef)
            {
                returnExpression = RefExpression(
                    PrefixUnaryExpression(
                        SyntaxKind.PointerIndirectionExpression,
                        returnExpression));
            }

            statements = statements.Add(ReturnStatement(returnExpression));
        }

        return Block(statements);

    }
    
    private static ArgumentSyntax GetArgumentForPInvokeParameter(ParameterStubGenerationContext ctx)
    {
        if (ctx.MarshallerShape != null)
        {
            return ctx.MarshallerShape.GetArgument(ctx);
        }
        else
        {
            ExpressionSyntax expr = IdentifierName(ctx.Symbol.Name);
            return WithPInvokeParameterRefToken(Argument(expr), ctx.Symbol);
        }
    }
    
    /// <summary>
    /// Generate a step in the marshalling process with generated code for each parameter of the current method
    /// </summary>
    private static SyntaxList<TNode> Step<TNode>(
        MethodStubGenerationContext ctx,
        string? comment, 
        Func<IParameterSymbol, IMarshallerShape, SyntaxList<TNode>> marshaller,
        SyntaxList<TNode> additional = default) where TNode : SyntaxNode
    {
        var result = List(ctx.Parameters.Where(x => x.MarshallerShape != null)
            .SelectMany(x => marshaller(x.Symbol, x.MarshallerShape!)));

        result = result.AddRange(additional);

        if (comment != null && result.Count > 0)
        {
            result = result.Replace(result[0],
                result[0]
                    .WithLeadingTrivia(Comment(comment)));
        }

        return result;
    }
    
    /// <summary>
    /// Generate a step in the marshalling process with generated code for each parameter of the current method
    /// </summary>
    private static SyntaxList<TNode> Step<TNode>(
        MethodStubGenerationContext ctx,
        string? comment, 
        Func<IParameterSymbol, IMarshallerShape, TNode?> marshaller) where TNode : SyntaxNode
    {
        var result = List(ctx.Parameters.Where(x => x.MarshallerShape != null)
                .Select(x => marshaller(x.Symbol, x.MarshallerShape!))
                .Where(x => x != null)
                .Select(x => x!)
            );

        if (comment != null && result.Count > 0)
        {
            result = result.Replace(result[0],
                result[0]
                    .WithLeadingTrivia(Comment(comment)));
        }

        return result;
    }

    /// <summary>
    /// Returns a local function declaration for the extern function.
    /// </summary>
    private static LocalFunctionStatementSyntax GenerateExternFunction(MethodStubGenerationContext ctx, TypeSyntax externReturnType)
    {
        var handleParam = Parameter(Identifier("handle_")).WithType(ParseTypeName("nint"));

        return HelperSyntaxFactory.GenerateExternFunction(
            library: ctx.Library, 
            externName: ToExternName(ctx),
            externReturnType: externReturnType, 
            parameters: ctx.Parameters.Select(x => ToForwardInfo(x.Symbol, x.MarshallerShape, true)), 
            parametersPrefix: handleParam);
    }

    /// <summary>
    /// Returns the external native name of a function.
    /// </summary>
    private static string ToExternName(MethodStubGenerationContext ctx)
    {
        var overload = ctx.Symbol.GetAttribute(Constants.OverloadAttributeFQN)?.ConstructorArguments[0].Value as string;

        return ctx.Symbol.GetAttribute(Constants.FunctionAttributeFQN)?.ConstructorArguments[0].Value is string functionName 
            ? $"{ctx.NativeTypeName}_{functionName}" 
            : $"{ctx.NativeTypeName}_{StringUtil.FirstCharToLower(ctx.Symbol.Name)}{overload}";
    }
    
}

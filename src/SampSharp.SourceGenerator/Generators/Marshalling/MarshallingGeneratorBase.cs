using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.Models;
using SampSharp.SourceGenerator.SyntaxFactories;

namespace SampSharp.SourceGenerator.Generators.Marshalling;

public abstract class MarshallingGeneratorBase
{
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

    private const string LocalInvokeSucceeded =  "__invokeSucceeded";

    /// <summary>
    /// Returns a block with the invocation of the native method including marshalling of parameters and return value.
    /// </summary>
    protected virtual BlockSyntax GetMarshallingBlock(MarshallingStubGenerationContext ctx)
    {
        return ctx.RequiresMarshalling 
            ? CreateInvocationWithMarshalling(ctx)
            : CreateInvocationWithoutMarshalling(ctx);
    }

    protected abstract ExpressionSyntax GetInvocation(MarshallingStubGenerationContext ctx);

    private BlockSyntax CreateInvocationWithoutMarshalling(MarshallingStubGenerationContext ctx)
    {
        var invoke = GetInvocation(ctx);

        if (ctx.Symbol.ReturnsVoid)
        {
            return SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(invoke));
        }
            
        if (ctx.Symbol.ReturnsByRef || ctx.Symbol.ReturnsByRefReadonly)
        {
            invoke = SyntaxFactory.RefExpression(invoke);
        }

        return SyntaxFactory.Block(SyntaxFactory.ReturnStatement(invoke));
    }

    protected IEnumerable<ArgumentSyntax> GetInvocationArgugments(MarshallingStubGenerationContext ctx)
    {
        return ctx.Parameters.Select(GetArgumentForPInvokeParameter);
    }

    private BlockSyntax CreateInvocationWithMarshalling(MarshallingStubGenerationContext ctx)
    {
        var invoke = GetInvocation(ctx);

        if (!ctx.Symbol.ReturnsVoid)
        {
            invoke = 
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression, 
                    SyntaxFactory.IdentifierName(ctx.ReturnMarshallerShape == null ? MarshallerConstants.LocalReturnValue : MarshallerHelper.GetNativeVar(null)), 
                    invoke);
        }

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
        var initLocals = Step(ctx, null, (p, m) => m.RequiresLocal 
            ? SyntaxFactory.SingletonList<StatementSyntax>(StatementFactory.CreateLocalDeclarationWithDefaultValue(m.GetNativeType(), MarshallerHelper.GetNativeVar(p))) 
            : []);

        if (!ctx.Symbol.ReturnsVoid)
        {
            var returnType = TypeSyntaxFactory.TypeNameGlobal(ctx.Symbol.ReturnType);
            
            if (ctx.ReturnsByRef)
            {
                returnType = SyntaxFactory.PointerType(returnType);
            }

            initLocals = initLocals.Add(StatementFactory.CreateLocalDeclarationWithDefaultValue(returnType, MarshallerConstants.LocalReturnValue));
            
            if (ctx.ReturnMarshallerShape != null)
            {
                var nativeType = ctx.ReturnMarshallerShape.GetNativeType();
                initLocals = initLocals.Add(StatementFactory.CreateLocalDeclarationWithDefaultValue(nativeType, MarshallerHelper.GetNativeVar(null)));
            }
        }

        // if callee cleanup or guaranteed unmarshalling is required, we need to keep track of invocation success
        if (cleanupCallee.Count > 0 || guaranteedUnmarshal.Count > 0)
        {
            // var __invokeSucceeded = false; to the initializer block
            initLocals = initLocals.Add(
                StatementFactory.CreateLocalDeclarationWithDefaultValue(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)), LocalInvokeSucceeded));

            // insert __invokeSucceeded = true; at the beginning of the notify step
            notify = notify.Insert(0, 
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression, 
                        SyntaxFactory.IdentifierName(LocalInvokeSucceeded), 
                        SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))));
        
            cleanupCallee = SyntaxFactory.SingletonList<StatementSyntax>(
                SyntaxFactory.IfStatement(SyntaxFactory.IdentifierName(LocalInvokeSucceeded), 
                    SyntaxFactory.Block(guaranteedUnmarshal.AddRange(cleanupCallee))));
        }

        // chain all pin statements
        StatementSyntax pinnedBlock = SyntaxFactory.Block(pinnedMarshal.Add(SyntaxFactory.ExpressionStatement(invoke)));
        for (var i = pin.Count - 1; i >= 0; i--)
        {
            pinnedBlock = pin[i].WithStatement(pinnedBlock);
        }
        
        // wire up steps
        var statements = initLocals.AddRange(setup);

        var tryBlock = marshal
            .Add(pinnedBlock)
            .AddRange(notify)
            .AddRange(unmarshalCapture)
            .AddRange(unmarshal);

        // only add try {} finally {} if there is something to clean up
        var finallyBlock = cleanupCallee.AddRange(cleanupCaller);
        if (finallyBlock.Any())
        {
            statements = statements.Add(SyntaxFactory.TryStatement()
                .WithBlock(SyntaxFactory.Block(tryBlock))
                .WithFinally(
                    SyntaxFactory.FinallyClause(
                        SyntaxFactory.Block(finallyBlock)))
            );
        }
        else
        {
            statements = statements.AddRange(tryBlock);
        }

        // add return statement if the method returns a value
        if (!ctx.Symbol.ReturnsVoid)
        {
            ExpressionSyntax returnExpression = SyntaxFactory.IdentifierName(MarshallerConstants.LocalReturnValue);

            if (ctx.ReturnsByRef)
            {
                returnExpression = SyntaxFactory.RefExpression(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.PointerIndirectionExpression,
                        returnExpression));
            }

            statements = statements.Add(SyntaxFactory.ReturnStatement(returnExpression));
        }

        return SyntaxFactory.Block(statements);

    }
    
    private static ArgumentSyntax GetArgumentForPInvokeParameter(ParameterStubGenerationContext ctx)
    {
        if (ctx.MarshallerShape != null)
        {
            return ctx.MarshallerShape.GetArgument(ctx);
        }

        ExpressionSyntax expr = SyntaxFactory.IdentifierName(ctx.Symbol.Name);
        return HelperSyntaxFactory.WithPInvokeParameterRefToken(SyntaxFactory.Argument(expr), ctx.Symbol);
    }
    
    /// <summary>
    /// Generate a step in the marshalling process with generated code for each parameter of the current method
    /// </summary>
    private static SyntaxList<TNode> Step<TNode>(
        MarshallingStubGenerationContext ctx,
        string? comment, 
        Func<IParameterSymbol, IMarshallerShape, SyntaxList<TNode>> marshaller,
        SyntaxList<TNode> additional = default) where TNode : SyntaxNode
    {
        var result = SyntaxFactory.List(ctx.Parameters.Where(x => x.MarshallerShape != null)
            .SelectMany(x => marshaller(x.Symbol, x.MarshallerShape!)));

        result = result.AddRange(additional);

        if (comment != null && result.Count > 0)
        {
            result = result.Replace(result[0],
                result[0]
                    .WithLeadingTrivia(SyntaxFactory.Comment(comment)));
        }

        return result;
    }
    
    /// <summary>
    /// Generate a step in the marshalling process with generated code for each parameter of the current method
    /// </summary>
    private static SyntaxList<TNode> Step<TNode>(
        MarshallingStubGenerationContext ctx,
        string? comment, 
        Func<IParameterSymbol, IMarshallerShape, TNode?> marshaller) where TNode : SyntaxNode
    {
        var result = SyntaxFactory.List(ctx.Parameters.Where(x => x.MarshallerShape != null)
            .Select(x => marshaller(x.Symbol, x.MarshallerShape!))
            .Where(x => x != null)
            .Select(x => x!)
        );

        if (comment != null && result.Count > 0)
        {
            result = result.Replace(result[0],
                result[0]
                    .WithLeadingTrivia(SyntaxFactory.Comment(comment)));
        }

        return result;
    }
}
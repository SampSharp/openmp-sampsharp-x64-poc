using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Marshalling;
using SampSharp.SourceGenerator.Marshalling.V2;
using SampSharp.SourceGenerator.Models;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.StatementFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators.Marshalling;

public abstract class MarshallingGeneratorBaseV2(MarshalDirection direction)
{
    private const string LocalInvokeSucceeded =  "__invokeSucceeded";

    /// <summary>
    /// Returns a block with the invocation of the native method including marshalling of parameters and return value.
    /// </summary>
    protected virtual BlockSyntax GetMarshallingBlock(MarshallingStubGenerationContext ctx)
    {
        return ctx.RequiresMarshalling 
            ? GenerateInvocationWithMarshalling(ctx)
            : GenerateInvocationWithoutMarshalling(ctx);
    }

    protected abstract ExpressionSyntax GetInvocation(MarshallingStubGenerationContext ctx);

    private BlockSyntax GenerateInvocationWithoutMarshalling(MarshallingStubGenerationContext ctx)
    {
        var invoke = GetInvocation(ctx);

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

    protected IEnumerable<ArgumentSyntax> GetInvocationArguments(MarshallingStubGenerationContext ctx)
    {
        return ctx.Parameters.Select(GetArgumentForPInvokeParameter);
    }

    private BlockSyntax GenerateInvocationWithMarshalling(MarshallingStubGenerationContext ctx)
    {
        return direction == MarshalDirection.ManagedToUnmanaged 
            ? GenerateInvocationWithMarshallingManagedToUnmanaged(ctx) 
            : GenerateInvocationWithMarshallingUnmanagedToManaged(ctx);
    }
    
    private BlockSyntax GenerateInvocationWithMarshallingManagedToUnmanaged(MarshallingStubGenerationContext ctx)
    {
        // The generated method consists of the following content:
        //
        // LocalsInit
        // Setup
        // try
        // {
        //   Marshal
        //   {
        //     PinnedMarshal
        //     Invoke 
        //   }
        //   [[__invokeSucceeded = true;]]
        //   NotifyForSuccessfulInvoke
        //   UnmarshalCapture
        //   Unmarshal
        // }
        // finally
        // {
        //   if (__invokeSucceeded)
        //   {
        //      GuaranteedUnmarshal
        //      CleanupCalleeAllocated
        //   }
        //   CleanupCallerAllocated
        // }
        //
        // return: retVal

        var steps = CollectPhases(ctx);

        var initLocals = List(GenerateInitLocals(ctx));

        // if callee cleanup or guaranteed unmarshalling is required, we need to keep track of invocation success
        var guaranteedStatements = steps.GuaranteedUnmarshal.AddRange(steps.CleanupCallee);
        var notify = steps.Notify;

        GenerateGuaranteedBlock(ref guaranteedStatements, ref initLocals, ref notify);

        // chain all pin statements
        var invoke = AddReturnValueAssignmentToInvoke(ctx, GetInvocation(ctx));
        var pinnedBlock = ChainPins(steps, invoke);

        // wire up steps
        var statements = initLocals
            .AddRange(steps.Setup)
            .AddRange(
                TryCatch(
                    tryBlock: steps.Marshal
                        .Add(pinnedBlock)
                        .AddRange(notify)
                        .AddRange(steps.UnmarshalCapture)
                        .AddRange(steps.Unmarshal),
                    finallyBlock: guaranteedStatements
                        .AddRange(steps.CleanupCaller)));

        // add return statement if the method returns a value
        if (!ctx.Symbol.ReturnsVoid)
        {
            ExpressionSyntax returnExpression = IdentifierName(MarshallerConstants.LocalReturnValue);

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

    private BlockSyntax GenerateInvocationWithMarshallingUnmanagedToManaged(MarshallingStubGenerationContext ctx)
    {
        // NOTE: We support only unmarshalling options for unmanaged to managed marshalling. 
        //
        // LocalsInit
        // Setup
        // try
        // {
        //   GuaranteedUnmarshal
        //   UnmarshalCapture
        //   Unmarshal
        //   {
        //     p/invoke 
        //   }
        //   [[__invokeSucceeded = true;]]
        //   NotifyForSuccessfulInvoke
        //   Marshal
        //   PinnedMarshal
        // }
        // finally
        // {
        //   CleanupCallerAllocated
        // }
        //
        // return: retVal

        var steps = CollectPhases(ctx);

        if (steps.CleanupCallee.Count > 0)
        {
            throw new InvalidOperationException("cannot cleanup callee allocated");
        }

        var initLocals = List(GenerateInitLocals(ctx));

        // if callee cleanup or guaranteed unmarshalling is required, we need to keep track of invocation success
        var notify = steps.Notify;

        var invoke = ExpressionStatement(AddReturnValueAssignmentToInvoke(ctx, GetInvocation(ctx)));

        // wire up steps
        var statements = initLocals
            .AddRange(steps.Setup)
            .AddRange(
                TryCatch(
                    tryBlock: steps.UnmarshalCapture
                        .AddRange(steps.Unmarshal)
                        .AddRange(steps.GuaranteedUnmarshal)
                        .Add(invoke)
                        .AddRange(notify),
                    finallyBlock: steps.CleanupCaller));

        // add return statement if the method returns a value
        if (!ctx.Symbol.ReturnsVoid)
        {
            ExpressionSyntax returnExpression = IdentifierName(MarshallerConstants.LocalReturnValue);

            if (ctx.ReturnsByRef)
            {
                // TODO: this may be wrong
                returnExpression = RefExpression(
                    PrefixUnaryExpression(
                        SyntaxKind.PointerIndirectionExpression,
                        returnExpression));
            }

            statements = statements.Add(ReturnStatement(returnExpression));
        }

        return Block(statements);
    }

    private static void GenerateGuaranteedBlock(
        ref SyntaxList<StatementSyntax> guarannteedStatements, 
        ref SyntaxList<StatementSyntax> initLocals, 
        ref SyntaxList<StatementSyntax> notify)
    {
        if (guarannteedStatements.Count > 0)
        {
            // var __invokeSucceeded = false; to the initializer block
            initLocals = initLocals.Add(GenerateInvokeSucceededLocal());

            // insert __invokeSucceeded = true; at the beginning of the notify step
            notify = notify.Insert(0, GenerateSetInvokeSucceeded());
        
            guarannteedStatements = SingletonList<StatementSyntax>(
                IfStatement(
                    IdentifierName(LocalInvokeSucceeded), 
                    Block(guarannteedStatements)));
        }
    }

    private IEnumerable<StatementSyntax> GenerateInitLocals(MarshallingStubGenerationContext ctx)
    {
        foreach(var p in ctx.Parameters)
        {
            if (!p.V2Ctx.Generator.UsesNativeIdentifier)
            {
                continue;
            }

            if (p.V2Ctx.Direction == MarshalDirection.ManagedToUnmanaged)
            {
                
                yield return CreateLocalDeclarationWithDefaultValue(p.V2Ctx.Generator.GetNativeType(p.V2Ctx), p.V2Ctx.GetNativeVar());
            }
            else
            {
                // UnmanagedToManaged
                yield return CreateLocalDeclarationWithDefaultValue(TypeNameGlobal(p.V2Ctx.ManagedType),  p.V2Ctx.GetManagedVar());
            }
        }
        

        if (!ctx.Symbol.ReturnsVoid)
        {
            
            yield return
                CreateLocalDeclarationWithDefaultValue(
                    GetReturnType(ctx),
                    MarshallerConstants.LocalReturnValue);
            
            
            if (ctx.ReturnV2Ctx.Generator.UsesNativeIdentifier)
            {
                // TODO: both dirs?
                yield return
                    CreateLocalDeclarationWithDefaultValue(
                        direction == MarshalDirection.ManagedToUnmanaged 
                            ? ctx.ReturnV2Ctx.Generator.GetNativeType(ctx.ReturnV2Ctx) 
                            : TypeNameGlobal(ctx.Symbol.ReturnType), 
                        GetVarName(null));
            }
        }
        
        string GetVarName(IParameterSymbol? parameterSymbol)
        {
            // TODO: GetVarName should go
            return direction == MarshalDirection.ManagedToUnmanaged
                ? MarshallerHelper.GetNativeVar(parameterSymbol)
                : MarshallerHelper.GetManagedVar(parameterSymbol);
        }
    }

    private static TypeSyntax GetReturnType(MarshallingStubGenerationContext ctx)
    {
        var returnType = TypeNameGlobal(ctx.Symbol.ReturnType);

        if (ctx.ReturnsByRef)
        {
            returnType = PointerType(returnType);
        }

        return returnType;
    }

    private static LocalDeclarationStatementSyntax GenerateInvokeSucceededLocal()
    {
        return CreateLocalDeclarationWithDefaultValue(PredefinedType(Token(SyntaxKind.BoolKeyword)), LocalInvokeSucceeded);
    }

    private static ExpressionStatementSyntax GenerateSetInvokeSucceeded()
    {
        return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression, 
                IdentifierName(LocalInvokeSucceeded), 
                LiteralExpression(SyntaxKind.TrueLiteralExpression)));
    }

    private static StatementSyntax ChainPins(MarshallingPhases phases, ExpressionSyntax invoke)
    {
        StatementSyntax pinnedBlock = Block(phases.PinnedMarshal.Add(ExpressionStatement(invoke)));
        for (var i = phases.Pin.Count - 1; i >= 0; i--)
        {
            if (phases.Pin[i] is not FixedStatementSyntax fixedStatement)
            {
                throw new InvalidOperationException("Pinned statement is not fixed statement");
            }

            pinnedBlock = fixedStatement.WithStatement(pinnedBlock);
        }

        return pinnedBlock;
    }

    private static SyntaxList<StatementSyntax> TryCatch(SyntaxList<StatementSyntax> tryBlock, SyntaxList<StatementSyntax> finallyBlock)
    {
        if (finallyBlock.Count == 0)
        {
            return tryBlock;
        }

        return SingletonList<StatementSyntax>(
            TryStatement()
                .WithBlock(Block(tryBlock))
                .WithFinally(
                    FinallyClause(
                        Block(finallyBlock))));
    }

    private static ExpressionSyntax AddReturnValueAssignmentToInvoke(MarshallingStubGenerationContext ctx, ExpressionSyntax invoke)
    {
        if (!ctx.Symbol.ReturnsVoid)
        {
            invoke = 
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression, 
                    IdentifierName(ctx.ReturnV2Ctx.Generator.UsesNativeIdentifier ? MarshallerHelper.GetNativeVar(null) : MarshallerConstants.LocalReturnValue), 
                    invoke);
        }

        return invoke;
    }
    
    private static MarshallingPhases CollectPhases(MarshallingStubGenerationContext ctx)
    {
        var setup = PhaseV2(ctx, MarshalPhase.Setup, MarshallingCodeGenDocumentation.COMMENT_SETUP);
        var marshal = PhaseV2(ctx, MarshalPhase.Marshal, MarshallingCodeGenDocumentation.COMMENT_MARSHAL);
        var pinnedMarshal = PhaseV2(ctx, MarshalPhase.PinnedMarshal, MarshallingCodeGenDocumentation.COMMENT_PINNED_MARSHAL);
        var pin = PhaseV2(ctx, MarshalPhase.Pin, MarshallingCodeGenDocumentation.COMMENT_PIN);
        var notify = PhaseV2(ctx, MarshalPhase.NotifyForSuccessfulInvoke, MarshallingCodeGenDocumentation.COMMENT_NOTIFY);
        var unmarshalCapture = PhaseV2(ctx, MarshalPhase.UnmarshalCapture, MarshallingCodeGenDocumentation.COMMENT_UNMARSHAL_CAPTURE);
        var unmarshal = PhaseV2(ctx, MarshalPhase.Unmarshal, MarshallingCodeGenDocumentation.COMMENT_UNMARSHAL);
        var guaranteedUnmarshal = PhaseV2(ctx, MarshalPhase.GuaranteedUnmarshal, MarshallingCodeGenDocumentation.COMMENT_GUARANTEED_UNMARSHAL);
        var cleanupCallee = PhaseV2(ctx, MarshalPhase.CleanupCalleeAllocated, MarshallingCodeGenDocumentation.COMMENT_CLEANUP_CALLEE);
        var cleanupCaller = PhaseV2(ctx, MarshalPhase.CleanupCallerAllocated, MarshallingCodeGenDocumentation.COMMENT_CLEANUP_CALLER);

        return new MarshallingPhases(setup, marshal, pinnedMarshal, pin, notify, unmarshalCapture, unmarshal, guaranteedUnmarshal, cleanupCallee, cleanupCaller);
    }

    private static ArgumentSyntax GetArgumentForPInvokeParameter(ParameterStubGenerationContext ctx)
    {
        
        var arg = ctx.V2Ctx.Parameter!.Name;
        if (ctx.V2Ctx.Generator.UsesNativeIdentifier)
        {
            arg = MarshallerHelper.GetNativeVar(ctx.Symbol);
        }
        
        ExpressionSyntax expr = IdentifierName(arg);

        if (ctx.Symbol.RefKind is RefKind.In or RefKind.RefReadOnlyParameter)
        {
            expr = PrefixUnaryExpression(SyntaxKind.AddressOfExpression, expr);
        }

        return HelperSyntaxFactory.WithPInvokeParameterRefToken(Argument(expr), ctx.Symbol);
    }
    
    private static SyntaxList<StatementSyntax> PhaseV2(
        MarshallingStubGenerationContext ctx,
        MarshalPhase phase,
        string? comment)
    {
        var elements = ctx.Parameters
            .SelectMany(x => x.V2Ctx.Generator.Generate(phase, x.V2Ctx))
            .Concat(ctx.ReturnV2Ctx.Generator.Generate(phase, ctx.ReturnV2Ctx));

        var result = List(elements);

        if (comment != null && result.Count > 0)
        {
            result = result.Replace(result[0],
                result[0]
                    .WithLeadingTrivia(Comment(comment)));
        }

        return result;
    }

    private record MarshallingPhases(
        SyntaxList<StatementSyntax> Setup,
        SyntaxList<StatementSyntax> Marshal,
        SyntaxList<StatementSyntax> PinnedMarshal,
        SyntaxList<StatementSyntax> Pin,
        SyntaxList<StatementSyntax> Notify,
        SyntaxList<StatementSyntax> UnmarshalCapture,
        SyntaxList<StatementSyntax> Unmarshal,
        SyntaxList<StatementSyntax> GuaranteedUnmarshal,
        SyntaxList<StatementSyntax> CleanupCallee,
        SyntaxList<StatementSyntax> CleanupCaller);

}
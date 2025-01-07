using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Helpers;
using SampSharp.SourceGenerator.Marshalling;
using SampSharp.SourceGenerator.Marshalling.Shapes;
using SampSharp.SourceGenerator.Models;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.StatementFactory;
using static SampSharp.SourceGenerator.SyntaxFactories.TypeSyntaxFactory;

namespace SampSharp.SourceGenerator.Generators.Marshalling;

public abstract class MarshallingGeneratorBase(MarshalDirection direction)
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

        var initLocals = GenerateInitLocals(ctx);

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

        var initLocals = GenerateInitLocals(ctx);

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

    private SyntaxList<StatementSyntax> GenerateInitLocals(MarshallingStubGenerationContext ctx)
    {
        var initLocals = Phase(ctx, null, (p, m) => m.RequiresLocal 
            ? SingletonList<StatementSyntax>(CreateLocalDeclarationWithDefaultValue(GetParamType(m, p), GetVar(p))) 
            : []);

        if (!ctx.Symbol.ReturnsVoid)
        {
            initLocals = initLocals.Add(
                CreateLocalDeclarationWithDefaultValue(
                    GetReturnType(ctx),
                    MarshallerConstants.LocalReturnValue));
            
            if (ctx.ReturnMarshallerShape != null)
            {
                initLocals = initLocals.Add(
                    CreateLocalDeclarationWithDefaultValue(
                        direction == MarshalDirection.ManagedToUnmanaged 
                            ? ctx.ReturnMarshallerShape.GetNativeType() 
                            : TypeNameGlobal(ctx.Symbol.ReturnType), 
                        GetVar(null)));
            }
        }

        return initLocals;

        TypeSyntax GetParamType(IMarshallerShape shape, IParameterSymbol p)
        {
            return direction == MarshalDirection.ManagedToUnmanaged
                ? shape.GetNativeType()
                : TypeNameGlobal(p.Type);
        }

        string GetVar(IParameterSymbol? parameterSymbol)
        {
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
            pinnedBlock = phases.Pin[i].WithStatement(pinnedBlock);
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
                    IdentifierName(ctx.ReturnMarshallerShape == null ? MarshallerConstants.LocalReturnValue : MarshallerHelper.GetNativeVar(null)), 
                    invoke);
        }

        return invoke;
    }
    
    private static MarshallingPhases CollectPhases(MarshallingStubGenerationContext ctx)
    {
        var setup = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_SETUP, (p, m) => m.Setup(p), ctx.ReturnMarshallerShape?.Setup(null) ?? default);
        var marshal = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_MARSHAL, (p, m) => m.Marshal(p), ctx.ReturnMarshallerShape?.Marshal(null) ?? default);
        var pinnedMarshal = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_PINNED_MARSHAL, (p, m) => m.PinnedMarshal(p), ctx.ReturnMarshallerShape?.PinnedMarshal(null) ?? default);
        var pin = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_PIN, (p, m) => m.Pin(p));
        var notify = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_NOTIFY, (p, m) => m.NotifyForSuccessfulInvoke(p), ctx.ReturnMarshallerShape?.NotifyForSuccessfulInvoke(null) ?? default);
        var unmarshalCapture = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_UNMARSHAL_CAPTURE, (p, m) => m.UnmarshalCapture(p), ctx.ReturnMarshallerShape?.UnmarshalCapture(null) ?? default);
        var unmarshal = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_UNMARSHAL, (p, m) => m.Unmarshal(p), ctx.ReturnMarshallerShape?.Unmarshal(null) ?? default);
        var guaranteedUnmarshal = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_GUARANTEED_UNMARSHAL, (p, m) => m.GuaranteedUnmarshal(p), ctx.ReturnMarshallerShape?.GuaranteedUnmarshal(null) ?? default);
        var cleanupCallee = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_CLEANUP_CALLEE, (p, m) => m.CleanupCalleeAllocated(p), ctx.ReturnMarshallerShape?.CleanupCalleeAllocated(null) ?? default);
        var cleanupCaller = Phase(ctx, MarshallingCodeGenDocumentation.COMMENT_CLEANUP_CALLER, (p, m) => m.CleanupCallerAllocated(p), ctx.ReturnMarshallerShape?.CleanupCallerAllocated(null) ?? default);

        return new MarshallingPhases(setup, marshal, pinnedMarshal, pin, notify, unmarshalCapture, unmarshal, guaranteedUnmarshal, cleanupCallee, cleanupCaller);
    }

    private static ArgumentSyntax GetArgumentForPInvokeParameter(ParameterStubGenerationContext ctx)
    {
        if (ctx.MarshallerShape != null)
        {
            return ctx.MarshallerShape.GetArgument(ctx);
        }

        ExpressionSyntax expr = IdentifierName(ctx.Symbol.Name);
        return HelperSyntaxFactory.WithPInvokeParameterRefToken(Argument(expr), ctx.Symbol);
    }
    
    private static SyntaxList<TNode> Phase<TNode>(
        MarshallingStubGenerationContext ctx,
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
    
    private static SyntaxList<TNode> Phase<TNode>(
        MarshallingStubGenerationContext ctx,
        string? comment, 
        Func<IParameterSymbol, IMarshallerShape, TNode?> marshaller) where TNode : SyntaxNode
    {
        var result = List(ctx.Parameters.Where(x => x.MarshallerShape != null)
            .Select(x => marshaller(x.Symbol, x.MarshallerShape!))
            .WhereNotNull()
        );

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
        SyntaxList<FixedStatementSyntax> Pin,
        SyntaxList<StatementSyntax> Notify,
        SyntaxList<StatementSyntax> UnmarshalCapture,
        SyntaxList<StatementSyntax> Unmarshal,
        SyntaxList<StatementSyntax> GuaranteedUnmarshal,
        SyntaxList<StatementSyntax> CleanupCallee,
        SyntaxList<StatementSyntax> CleanupCaller);

}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.Models;
using SampSharp.SourceGenerator.SyntaxFactories;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes;

public abstract class MarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType) : IMarshallerShape
{
    protected ITypeSymbol NativeType => nativeType;
    protected ITypeSymbol MarshallerType => marshallerType;
    protected string NativeTypeName { get; } = TypeSyntaxFactory.ToGlobalTypeString(nativeType);
    protected string MarshallerTypeName  { get; } = TypeSyntaxFactory.ToGlobalTypeString(marshallerType);

    public virtual bool RequiresLocal => true;

    public virtual TypeSyntax GetNativeType()
    {
        return ParseTypeName(NativeTypeName);
    }

    public virtual SyntaxList<StatementSyntax> Setup(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual FixedStatementSyntax? Pin(IParameterSymbol? parameterSymbol)
    {
        return null;
    }

    public virtual SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual ArgumentSyntax GetArgument(ParameterStubGenerationContext ctx)
    {
        ExpressionSyntax expr = IdentifierName(GetNativeVar(ctx.Symbol));

        if (ctx.Symbol.RefKind is RefKind.In or RefKind.RefReadOnlyParameter)
        {
            expr = PrefixUnaryExpression(SyntaxKind.AddressOfExpression, expr);
        }

        return HelperSyntaxFactory.WithPInvokeParameterRefToken(Argument(expr), ctx.Symbol);
    }

    public virtual SyntaxList<StatementSyntax> GuaranteedUnmarshal(IParameterSymbol? parameterSymbol)
    {
        return List<StatementSyntax>();
    }
    
    protected static string GetManagedVar(IParameterSymbol? parameterSymbol)
    {
        return MarshallerHelper.GetManagedVar(parameterSymbol);
    }
    
    protected static string GetMarshallerVar(IParameterSymbol? parameterSymbol)
    {
        return MarshallerHelper.GetMarshallerVar(parameterSymbol);
    }

    protected static string GetNativeVar(IParameterSymbol? parameterSymbol)
    {
        return MarshallerHelper.GetNativeVar(parameterSymbol);
    }

    protected static string GetNativeExtraVar(IParameterSymbol? parameterSymbol, string extra)
    {
        return MarshallerHelper.GetNativeExtraVar(parameterSymbol, extra);
    }
}
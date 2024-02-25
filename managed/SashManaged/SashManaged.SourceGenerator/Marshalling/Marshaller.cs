using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public abstract class Marshaller(string nativeTypeName, string marshallerTypeName) : IMarshaller
{
    protected string NativeTypeName => nativeTypeName;
    protected string MarshallerTypeName => marshallerTypeName;

    public virtual TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol)
    {
        return ParseTypeName(nativeTypeName);
    }

    public virtual SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }
    
    public virtual SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }
    
    protected static string GetManagedVar(IParameterSymbol parameterSymbol)
    {
        return parameterSymbol?.Name ?? "__retVal";
    }

    protected static string GetUnmanagedVar(IParameterSymbol parameterSymbol)
    {
        return $"__{(parameterSymbol?.Name ?? "__retVal")}_native";
    }

    protected static string GetMarshallerVar(IParameterSymbol parameterSymbol)
    {
        return $"__{parameterSymbol.Name}_native_marshaller";
    }

    protected SyntaxList<StatementSyntax> InvokeAndAssign(string local, string method, string argument)
    {
        return SingletonList<StatementSyntax>(
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(local),
                    InvokeWithArgument(method, argument))));
    }

    protected InvocationExpressionSyntax InvokeWithArgument(string method, string argument)
    {
        return InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(marshallerTypeName),
                    IdentifierName(method)
                )
            )
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(
                        Argument(IdentifierName(argument))
                    )
                )
            );
    }
}
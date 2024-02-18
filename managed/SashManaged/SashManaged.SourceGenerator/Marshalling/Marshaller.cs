using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public abstract class Marshaller : IMarshaller
{
    public abstract bool RequiresMarshalling { get; }
    public abstract bool RequiresUnsafe { get; }
    public abstract TypeSyntax GetExternalType(ITypeSymbol typeSymbol);
    public virtual SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> ManagedToUnmanaged(IParameterSymbol parameter)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Free(IParameterSymbol parameter)
    {
        return List<StatementSyntax>();
    }

    public virtual ExpressionSyntax UnmanagedToManaged(ExpressionSyntax expression)
    {
        return expression;
    }

    public virtual ArgumentSyntax GetArgument(IParameterSymbol parameter)
    {
        return WithParameterRefKind(Argument(IdentifierName(parameter.Name)), parameter);
    }

    protected ArgumentSyntax WithParameterRefKind(ArgumentSyntax argument, IParameterSymbol parameter)
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
}
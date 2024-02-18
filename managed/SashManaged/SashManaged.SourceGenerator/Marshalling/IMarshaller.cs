using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public interface IMarshaller
{
    bool RequiresMarshalling { get; }
    bool RequiresUnsafe { get; }
    TypeSyntax GetExternalType(ITypeSymbol typeSymbol);
    SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter);
    SyntaxList<StatementSyntax> ManagedToUnmanaged(IParameterSymbol parameter);
    SyntaxList<StatementSyntax> Free(IParameterSymbol parameter);
    ExpressionSyntax UnmanagedToManaged(ExpressionSyntax expression);
    ArgumentSyntax GetArgument(IParameterSymbol parameter);
}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public interface IMarshaller
{
    TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol);
    
    SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter);
    SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol);
}
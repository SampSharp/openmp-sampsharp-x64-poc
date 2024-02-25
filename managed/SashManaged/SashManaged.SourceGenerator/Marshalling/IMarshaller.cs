using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public interface IMarshaller
{
    TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol);
    
    SyntaxList<StatementSyntax> Setup(IParameterSymbol parameter);
    SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol parameterSymbol);
    SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol parameterSymbol);
}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SashManaged.SourceGenerator.Models;

namespace SashManaged.SourceGenerator.Marshalling.Shapes;

public interface IMarshallerShape
{
    TypeSyntax GetNativeType();

    SyntaxList<StatementSyntax> Setup(IParameterSymbol? parameterSymbol);
    SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol);
    SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol? parameterSymbol);
    FixedStatementSyntax? Pin(IParameterSymbol? parameterSymbol);
    SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol? parameterSymbol);
    SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol);
    SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol);
    SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol? parameterSymbol);
    SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol? parameterSymbol);
    SyntaxList<StatementSyntax> GuaranteedUnmarshal(IParameterSymbol? parameterSymbol);

    bool RequiresLocal { get; }

    ArgumentSyntax GetArgument(ParameterStubGenerationContext ctx);
}
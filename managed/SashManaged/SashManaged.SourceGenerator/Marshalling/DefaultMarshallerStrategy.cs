using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public class DefaultMarshallerStrategy(string nativeTypeName, string marshallerTypeName, bool hasFree) : Marshaller
{
    public override TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol)
    {
        return ParseTypeName(nativeTypeName);
    }
    
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign(Unmanaged(parameterSymbol), Managed(parameterSymbol), marshallerTypeName, "ConvertToUnmanaged");
    }
    
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign(Managed(parameterSymbol), Unmanaged(parameterSymbol), marshallerTypeName, "ConvertToManaged");
    }

    public override SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol)
    {
        return !hasFree
            ? List<StatementSyntax>()
            : SingletonList<StatementSyntax>(
                ExpressionStatement(
                    InvokeWithArgument(marshallerTypeName, "Free", Unmanaged(parameterSymbol))));
    }
}
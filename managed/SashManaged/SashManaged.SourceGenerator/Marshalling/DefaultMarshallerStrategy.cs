using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public class DefaultMarshallerStrategy(string nativeTypeName, string marshallerTypeName, bool hasFree) : Marshaller
{
    public override TypeSyntax ToMarshalledType(ITypeSymbol typeSymbol)
    {
        return SyntaxFactory.ParseTypeName(nativeTypeName);
    }
    
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign($"__{parameterSymbol.Name}_native", parameterSymbol.Name, marshallerTypeName, "ConvertToUnmanaged");
    }
    
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return parameterSymbol == null
            ? InvokeAndAssign("__retVal", "__retVal_native", marshallerTypeName, "ConvertToManaged")
            : InvokeAndAssign(parameterSymbol.Name, $"__{parameterSymbol.Name}_native", marshallerTypeName, "ConvertToManaged");
    }

    public override SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol)
    {
        return !hasFree
            ? SyntaxFactory.List<StatementSyntax>()
            : SyntaxFactory.SingletonList<StatementSyntax>(
                SyntaxFactory.ExpressionStatement(
                    InvokeWithArgument(marshallerTypeName, "Free", $"__{parameterSymbol.Name}_native")));
    }
}
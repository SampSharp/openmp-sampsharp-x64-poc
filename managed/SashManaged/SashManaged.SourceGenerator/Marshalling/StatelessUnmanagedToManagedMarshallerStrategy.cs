using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public class StatelessUnmanagedToManagedMarshallerStrategy(string nativeTypeName, string marshallerTypeName, bool hasFree) : Marshaller(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign(GetManagedVar(parameterSymbol), "ConvertToManaged", GetUnmanagedVar(parameterSymbol));
    }

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol parameterSymbol)
    {
        // type.Free(unmanaged);
        return !hasFree
            ? SyntaxFactory.List<StatementSyntax>()
            : SyntaxFactory.SingletonList<StatementSyntax>(
                SyntaxFactory.ExpressionStatement(
                    InvokeWithArgument("Free", GetUnmanagedVar(parameterSymbol))));
    }
}
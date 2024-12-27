using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling.Stateless;

/// <summary>
/// Stateless Unmanaged->Managed with Guaranteed Unmarshalling
/// </summary>
public class StatelessUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(string nativeTypeName, string marshallerTypeName, bool hasFree) : StatelessMarshallerShape(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> GuaranteedUnmarshal(IParameterSymbol? parameterSymbol)
    {
        return InvokeAndAssign(GetManagedVar(parameterSymbol), "ConvertToManagedFinally", GetUnmanagedVar(parameterSymbol));
    }

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
    {
        // type.Free(unmanaged);
        return !hasFree
            ? SyntaxFactory.List<StatementSyntax>()
            : SyntaxFactory.SingletonList<StatementSyntax>(
                SyntaxFactory.ExpressionStatement(
                    InvokeWithArgument("Free", GetUnmanagedVar(parameterSymbol))));
    }
}
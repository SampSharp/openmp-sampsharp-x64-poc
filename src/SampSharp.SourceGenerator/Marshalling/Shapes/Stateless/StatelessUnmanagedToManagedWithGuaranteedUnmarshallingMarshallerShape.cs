using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

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
            ? List<StatementSyntax>()
            : SingletonList<StatementSyntax>(
                ExpressionStatement(
                    InvokeWithArgument("Free", GetUnmanagedVar(parameterSymbol))));
    }
}
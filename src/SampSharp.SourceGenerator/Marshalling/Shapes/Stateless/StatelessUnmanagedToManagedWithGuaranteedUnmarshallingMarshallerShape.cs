using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

/// <summary>
/// Stateless Unmanaged->Managed with Guaranteed Unmarshalling
/// </summary>
public class StatelessUnmanagedToManagedWithGuaranteedUnmarshallingMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType, bool hasFree, MarshalDirection direction) : StatelessMarshallerShape(nativeType, marshallerType, direction)
{
    public override SyntaxList<StatementSyntax> GuaranteedUnmarshal(IParameterSymbol? parameterSymbol)
    {
        return InvokeAndAssign(
            GetManagedVar(parameterSymbol), 
            ShapeConstants.MethodConvertToManagedFinally, 
            GetNativeVar(parameterSymbol));
    }

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
    {
        // type.Free(unmanaged);
        return !hasFree
            ? List<StatementSyntax>()
            : SingletonList<StatementSyntax>(
                ExpressionStatement(
                    InvokeWithArgument(
                        ShapeConstants.MethodFree, 
                        GetNativeVar(parameterSymbol))));
    }
}
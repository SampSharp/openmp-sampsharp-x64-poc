using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

/// <summary>
/// Stateless Unmanaged->Managed
/// </summary>
public class StatelessUnmanagedToManagedMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType, bool hasFree, MarshalDirection direction) : StatelessMarshallerShape(nativeType, marshallerType, direction)
{
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol)
    {
        return InvokeAndAssign(GetManagedVar(parameterSymbol), ShapeConstants.MethodConvertToManaged, GetNativeVar(parameterSymbol));
    }

    public override SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
    {
        // type.Free(unmanaged);
        return !hasFree
            ? List<StatementSyntax>()
            : SingletonList<StatementSyntax>(
                ExpressionStatement(
                    InvokeWithArgument(ShapeConstants.MethodFree, GetNativeVar(parameterSymbol))));
    }
}
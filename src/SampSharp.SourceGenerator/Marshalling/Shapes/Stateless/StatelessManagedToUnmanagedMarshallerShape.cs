using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

/// <summary>
/// Stateless Managed->Unmanaged
/// </summary>
public class StatelessManagedToUnmanagedMarshallerShape(ITypeSymbol nativeType, ITypeSymbol marshallerType, bool hasFree) : StatelessMarshallerShape(nativeType, marshallerType)
{
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {
        return InvokeAndAssign(GetNativeVar(parameterSymbol), ShapeConstants.MethodConvertToUnmanaged, GetManagedVar(parameterSymbol));
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
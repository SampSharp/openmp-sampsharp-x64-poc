using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public class StatelessBidirectionalMarshallerStrategy(string nativeTypeName, string marshallerTypeName, bool hasFree) : Marshaller(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign(Unmanaged(parameterSymbol), Managed(parameterSymbol), "ConvertToUnmanaged");
    }
    
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign(Managed(parameterSymbol), Unmanaged(parameterSymbol), "ConvertToManaged");
    }

    public override SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol)
    {
        return !hasFree
            ? List<StatementSyntax>()
            : SingletonList<StatementSyntax>(
                ExpressionStatement(
                    InvokeWithArgument("Free", Unmanaged(parameterSymbol))));
    }
}

public class StatelessManagedToUnmanagedMarshallerStrategy(string nativeTypeName, string marshallerTypeName, bool hasFree) : Marshaller(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign(Unmanaged(parameterSymbol), Managed(parameterSymbol), "ConvertToUnmanaged");
    }
    
    public override SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol)
    {
        return !hasFree
            ? List<StatementSyntax>()
            : SingletonList<StatementSyntax>(
                ExpressionStatement(
                    InvokeWithArgument("Free", Unmanaged(parameterSymbol))));
    }
}

public class StatelessUnmanagedToManagedMarshallerStrategy(string nativeTypeName, string marshallerTypeName, bool hasFree) : Marshaller(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return InvokeAndAssign(Managed(parameterSymbol), Unmanaged(parameterSymbol), "ConvertToManaged");
    }

    public override SyntaxList<StatementSyntax> Cleanup(IParameterSymbol parameterSymbol)
    {
        return !hasFree
            ? List<StatementSyntax>()
            : SingletonList<StatementSyntax>(
                ExpressionStatement(
                    InvokeWithArgument("Free", Unmanaged(parameterSymbol))));
    }
}
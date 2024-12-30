﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling.Shapes.Stateless;

/// <summary>
/// Stateless Unmanaged->Managed
/// </summary>
public class StatelessUnmanagedToManagedMarshallerShape(string nativeTypeName, string marshallerTypeName, bool hasFree) : StatelessMarshallerShape(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol)
    {
        return InvokeAndAssign(GetManagedVar(parameterSymbol), "ConvertToManaged", GetUnmanagedVar(parameterSymbol));
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
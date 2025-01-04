﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SampSharp.SourceGenerator.Marshalling.Shapes.Stateless;

/// <summary>
/// Stateless Managed->Unmanaged
/// </summary>
public class StatelessManagedToUnmanagedMarshallerShape(string nativeTypeName, string marshallerTypeName, bool hasFree) : StatelessMarshallerShape(nativeTypeName, marshallerTypeName)
{
    public override SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {
        return InvokeAndAssign(GetUnmanagedVar(parameterSymbol), "ConvertToUnmanaged", GetManagedVar(parameterSymbol));
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
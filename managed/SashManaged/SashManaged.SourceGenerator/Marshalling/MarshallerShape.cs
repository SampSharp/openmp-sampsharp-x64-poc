﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public abstract class MarshallerShape(string nativeTypeName, string marshallerTypeName) : IMarshallerShape
{
    protected string NativeTypeName => nativeTypeName;
    protected string MarshallerTypeName => marshallerTypeName;

    public virtual TypeSyntax GetNativeType()
    {
        return ParseTypeName(nativeTypeName);
    }

    public virtual SyntaxList<StatementSyntax> Setup(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Marshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }
    
    public virtual SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }

    public virtual SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol parameterSymbol)
    {
        return List<StatementSyntax>();
    }
    
    protected static string GetManagedVar(IParameterSymbol parameterSymbol)
    {
        return parameterSymbol?.Name ?? "__retVal";
    }

    protected static string GetUnmanagedVar(IParameterSymbol parameterSymbol)
    {
        return $"__{(parameterSymbol?.Name ?? "retVal")}_native";
    }
}
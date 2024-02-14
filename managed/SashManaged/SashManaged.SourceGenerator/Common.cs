using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator;

public static class Common
{
    public static string DllImportAttribute(string lib, string callConv = "Cdecl")
    {
        return $"[System.Runtime.InteropServices.DllImport(\"{lib}\", CallingConvention = System.Runtime.InteropServices.CallingConvention.{callConv})]";
    }

    public static string GetForwardArguments(IMethodSymbol methodSymbol, bool marshall = false)
    {
        return string.Join(", ", methodSymbol.Parameters.Select(x => ToArgumentText(x, marshall)));
    }

    private static string ToArgumentText(IParameterSymbol x, bool marshall)
    {
        if (marshall && x.HasAttribute("SashManaged.MarshallAttribute"))
        {
            return $"{x.Name}_";
        }

        return $"{RefArgumentString(x.RefKind)}{x.Name}";
    }

    public static string ParameterAsString(ImmutableArray<IParameterSymbol> parameters, bool marshal = false)
    {
        return string.Join(", ", parameters.Select(x => ToParameterText(x, marshal)));
    }

    private static string ToParameterText(IParameterSymbol x, bool marshal)
    {
        if (marshal)
        {
            return $"nint {x.Name}";
        }
        return x.ToDisplayString();
    }

    public static string RefArgumentString(RefKind kind)
    {
        return kind switch
        {
            RefKind.Out => "out ",
            RefKind.Ref => "ref ",
            RefKind.RefReadOnlyParameter => "ref ",
            RefKind.In => "in ",
            RefKind.None => string.Empty,
            _ => string.Empty
        };
    }
}
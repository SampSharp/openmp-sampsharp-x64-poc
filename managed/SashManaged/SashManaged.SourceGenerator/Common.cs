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

    public static string GetForwardArguments(IMethodSymbol methodSymbol, bool marshall = false, bool blittable = false)
    {
        return string.Join(", ", methodSymbol.Parameters.Select(x => ToArgumentText(x, marshall, blittable)));
    }

    private static string ToArgumentText(IParameterSymbol x, bool marshall, bool blittable)
    {
        if (blittable && x.RefKind == RefKind.Ref)
        {
            return $"ref (({Constants.BlittableRefFQN}<{x.Type.ToDisplayString()}>){x.Name}).Value";
        }
        if (marshall && x.HasAttribute(Constants.MarshallAttributeFQN))
        {
            return $"{x.Name}_";
        }

        return $"{RefArgumentString(x.RefKind)}{x.Name}";
    }

    public static string ToBlittableTypeString(ITypeSymbol type)
    {
        if (type.SpecialType == SpecialType.System_Boolean)
        {
            return Constants.BlittableBooleanFQN;
        }

        return type.ToDisplayString();
    }
    
    public static string ParameterAsString(ImmutableArray<IParameterSymbol> parameters, bool marshal = false, bool blittable = false)
    {
        return string.Join(", ", parameters.Select(x => ToParameterText(x, marshal, blittable)));
    }

    private static string ToParameterText(IParameterSymbol x, bool marshal, bool blittable)
    {
        if (blittable && x.RefKind == RefKind.Ref)
        {
            return $"nint {x.Name}";
        }
        if (blittable && x.Type.SpecialType == SpecialType.System_Boolean)
        {
            return $"{Constants.BlittableBooleanFQN} {x.Name}";
        }
        if (marshal && x.HasAttribute(Constants.MarshallAttributeFQN))
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
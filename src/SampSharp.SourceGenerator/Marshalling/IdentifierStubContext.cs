using Microsoft.CodeAnalysis;

namespace SampSharp.SourceGenerator.Marshalling;

public record IdentifierStubContext(
    IParameterSymbol? Parameter, // TODO: make parameter disappear to avoid null checks
    MarshalDirection Direction,
    ITypeSymbol ManagedType,
    CustomMarshallerInfo? Marshaller,
    MarshalMembers? MarshallerMembers,
    MarshallerShape Shape,
    IMarshalShapeGenerator Generator)
{
    public string GetManagedVar()
    {
        return MarshallerHelper.GetVar(Parameter);
    }

    public string GetMarshallerVar()
    {
        return MarshallerHelper.GetMarshallerVar(Parameter);
    }

    public string GetNativeVar()
    {
        return MarshallerHelper.GetNativeVar(Parameter);
    }

    public string GetNativeExtraVar(string extra)
    {
        return MarshallerHelper.GetNativeExtraVar(Parameter, extra);
    }
}
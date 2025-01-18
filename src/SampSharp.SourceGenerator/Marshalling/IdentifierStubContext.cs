using Microsoft.CodeAnalysis;

namespace SampSharp.SourceGenerator.Marshalling;

public record IdentifierStubContext(
    MarshalDirection Direction,
    ManagedType ManagedType,
    ManagedType? MarshallerType, 
    ManagedType? NativeType,
    MarshallerShape Shape,
    IMarshalShapeGenerator Generator,
    RefKind RefKind,
    string ManagedIdentifier)
{
    public string GetManagedId()
    {
        return ManagedIdentifier;
    }

    public string GetMarshallerId()
    {
        return GetNativeExtraId("marshaller");
    }

    public string GetNativeId()
    {
        return ManagedIdentifier == MarshallerConstants.LocalReturnValue 
            ? $"{MarshallerConstants.LocalReturnValue}_native" 
            : $"__{ManagedIdentifier}_native";
    }

    public string GetNativeExtraId(string extra)
    {
        return $"{GetNativeId()}__{extra}";
    }
}
using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator;

public static class MarshallingCodeGenerator
{
    public static WellKnownMarshallerTypes GetWellKnownMarshallerTypes(Compilation compilation)
    {
        var stringViewMarshaller = compilation.GetTypeByMetadataName("SashManaged.StringViewMarshaller");
        var booleanMarshaller = compilation.GetTypeByMetadataName("SashManaged.BooleanMarshaller");

        var wellKnownMarshallerTypes = new WellKnownMarshallerTypes(
            (x => x.SpecialType == SpecialType.System_String, stringViewMarshaller),
            (x => x.SpecialType == SpecialType.System_Boolean, booleanMarshaller)
        );
        return wellKnownMarshallerTypes;
    }
}
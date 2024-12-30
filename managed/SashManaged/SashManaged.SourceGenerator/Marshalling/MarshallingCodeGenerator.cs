using Microsoft.CodeAnalysis;
using SashManaged.SourceGenerator.Marshalling;

namespace SashManaged.SourceGenerator;

public static class MarshallingCodeGenerator
{
    public static WellKnownMarshallerTypes GetWellKnownMarshallerTypes(Compilation compilation)
    {
        var stringViewMarshaller = compilation.GetTypeByMetadataName(Constants.StringViewMarshallerFQN);

        var wellKnownMarshallerTypes = new WellKnownMarshallerTypes(
            (x => x.SpecialType == SpecialType.System_String, stringViewMarshaller)
        );
        return wellKnownMarshallerTypes;
    }
}
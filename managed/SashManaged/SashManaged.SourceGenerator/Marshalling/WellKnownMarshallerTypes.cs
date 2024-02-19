namespace SashManaged.SourceGenerator.Marshalling;

public static class WellKnownMarshallerTypes
{
    public static IMarshaller String { get; } = 
        new DefaultMarshallerStrategy(
            nativeTypeName: $"global::{Constants.StringViewFQN}",
            marshallerTypeName: "global::SashManaged.StringViewMarshaller",
            hasFree: true);

    public static IMarshaller Boolean { get; } = 
        new DefaultMarshallerStrategy(
            nativeTypeName: $"global::{Constants.BlittableBooleanFQN}",
            marshallerTypeName: "global::SashManaged.BooleanMarshaller",
            hasFree: false);
}
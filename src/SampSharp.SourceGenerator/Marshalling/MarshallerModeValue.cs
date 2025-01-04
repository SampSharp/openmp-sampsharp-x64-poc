namespace SampSharp.SourceGenerator.Marshalling;

public enum MarshallerModeValue
{
    Default,
    ManagedToUnmanagedIn,
    ManagedToUnmanagedRef,
    ManagedToUnmanagedOut,
    UnmanagedToManagedIn,
    UnmanagedToManagedRef,
    UnmanagedToManagedOut,
    Other
}
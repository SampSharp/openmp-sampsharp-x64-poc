namespace SampSharp.SourceGenerator.Marshalling;

public record MarshallingDirectionInfo(MarshallerModeValue In, MarshallerModeValue Out, MarshallerModeValue Ref)
{
    public static MarshallingDirectionInfo ManagedToUnmanaged =
        new(MarshallerModeValue.ManagedToUnmanagedIn, 
            MarshallerModeValue.ManagedToUnmanagedOut, 
            MarshallerModeValue.ManagedToUnmanagedRef);
    
    public static MarshallingDirectionInfo UnmanagedToManaged =
        new(MarshallerModeValue.UnmanagedToManagedIn, 
            MarshallerModeValue.UnmanagedToManagedOut, 
            MarshallerModeValue.UnmanagedToManagedRef);
}
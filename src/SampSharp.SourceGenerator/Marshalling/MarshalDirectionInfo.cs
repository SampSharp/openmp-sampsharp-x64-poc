namespace SampSharp.SourceGenerator.Marshalling;

public record MarshalDirectionInfo(MarshalMode In, MarshalMode Out, MarshalMode Ref)
{
    public static MarshalDirectionInfo ManagedToUnmanaged =
        new(MarshalMode.ManagedToUnmanagedIn, 
            MarshalMode.ManagedToUnmanagedOut, 
            MarshalMode.ManagedToUnmanagedRef);
    
    public static MarshalDirectionInfo UnmanagedToManaged =
        new(MarshalMode.UnmanagedToManagedIn, 
            MarshalMode.UnmanagedToManagedOut, 
            MarshalMode.UnmanagedToManagedRef);
}
namespace SampSharp.SourceGenerator.Marshalling.V2;

public enum MarshalPhase
{
    Setup,
    Marshal,
    PinnedMarshal,
    Pin,
    NotifyForSuccessfulInvoke,
    UnmarshalCapture,
    Unmarshal,
    CleanupCalleeAllocated,
    CleanupCallerAllocated,
    GuaranteedUnmarshal
}
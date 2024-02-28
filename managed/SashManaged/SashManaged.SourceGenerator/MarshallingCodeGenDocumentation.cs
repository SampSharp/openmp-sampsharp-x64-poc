namespace SashManaged.SourceGenerator;

public static class MarshallingCodeGenDocumentation
{
    public const string COMMENT_SETUP = "// Setup - Perform required setup.";
    public const string COMMENT_MARSHAL = "// Marshal - Convert managed data to native data.";
    public const string COMMENT_PINNED_MARSHAL = "// PinnedMarshal - Convert managed data to native data that requires the managed data to be pinned.";
    public const string COMMENT_NOTIFY = "// NotifyForSuccessfulInvoke - Keep alive any managed objects that need to stay alive across the call.";
    public const string COMMENT_UNMARSHAL_CAPTURE =
        "// UnmarshalCapture - Capture the native data into marshaller instances in case conversion to managed data throws an exception.";
    public const string COMMENT_UNMARSHAL = "// Unmarshal - Convert native data to managed data.";
    public const string COMMENT_CLEANUP_CALLEE = "// CleanupCalleeAllocated - Perform cleanup of callee allocated resources.";
    public const string COMMENT_CLEANUP_CALLER = "// CleanupCallerAllocated - Perform cleanup of caller allocated resources.";
    public const string COMMENT_P_INVOKE = "// Local P/Invoke";
}
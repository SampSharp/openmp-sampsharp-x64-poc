using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PeerRequestParams
{
    public readonly ClientVersion Version;
    public readonly StringView VersionName;
    public readonly bool Bot;
    public readonly StringView Name;
    public readonly StringView Serial;
    public readonly bool IsUsingOfficialClient;
};
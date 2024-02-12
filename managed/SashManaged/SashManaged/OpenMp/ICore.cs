using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct ICore
{
    private readonly nint _data;

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern SemanticVersion ICore_getVersion(ICore core);

    public SemanticVersion GetVersion()
    {
        return ICore_getVersion(this);
    }

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern void ICore_setData(ICore core, SettableCoreDataType type, StringView data);

    public void SetData(SettableCoreDataType type, StringView data)
    {
        ICore_setData(this, type, data);
    }

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern IPlayerPool ICore_getPlayers(ICore core);

    public IPlayerPool GetPlayers()
    {
        return ICore_getPlayers(this);
    }
}
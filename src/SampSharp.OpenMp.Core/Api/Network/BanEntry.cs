using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core.Api;

[NativeMarshalling(typeof(BanEntryMarshaller))]
public record BanEntry(string Address, DateTimeOffset Time, string? Name, string? Reason)
{
    public BanEntry(string address) : this(address, DateTimeOffset.UtcNow, null, null)
    {
    }
}
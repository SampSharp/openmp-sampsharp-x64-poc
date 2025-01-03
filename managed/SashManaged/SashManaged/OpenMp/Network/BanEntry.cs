using System.Runtime.InteropServices.Marshalling;

namespace SashManaged.OpenMp;

[NativeMarshalling(typeof(BanEntryMarshaller))]
public record BanEntry(string Address, DateTimeOffset Time, string Name, string Reason);
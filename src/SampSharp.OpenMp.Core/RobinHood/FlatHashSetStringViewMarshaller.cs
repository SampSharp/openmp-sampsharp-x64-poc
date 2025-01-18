using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core;

[CustomMarshaller(typeof(IEnumerable<string>), MarshalMode.UnmanagedToManagedIn, typeof(FlatHashSetStringViewMarshaller))]
public static class FlatHashSetStringViewMarshaller
{
    public static IEnumerable<string> ConvertToManaged(FlatHashSetStringView set)
    {
        return set;
    }
}
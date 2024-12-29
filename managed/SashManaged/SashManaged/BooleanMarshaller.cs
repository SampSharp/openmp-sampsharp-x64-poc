using System.Runtime.InteropServices.Marshalling;

namespace SashManaged;

[CustomMarshaller(typeof(bool), MarshalMode.Default, typeof(BooleanMarshaller))]
public static class BooleanMarshaller
{
    public static BlittableBoolean ConvertToUnmanaged(bool managed)
    {
        return managed;
    }

    public static bool ConvertToManaged(BlittableBoolean unmanaged)
    {
        return unmanaged;
    }
}
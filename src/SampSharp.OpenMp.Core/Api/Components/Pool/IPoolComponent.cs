namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPoolComponent{T}" /> interface.
/// </summary>
[OpenMpApi(typeof(IComponent))]
[OpenMpApiPartial]
public readonly partial struct IPoolComponent<T> where T : unmanaged, IIDProviderInterface
{
    public IPool<T> AsPool()
    {
        return new IPool<T>(IPoolComponentInterop.cast_IPoolComponent_to_IPool(_handle));
    }
}
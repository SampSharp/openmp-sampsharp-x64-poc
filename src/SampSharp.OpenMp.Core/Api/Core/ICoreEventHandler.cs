namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface ICoreEventHandler
{
    void OnTick(Microseconds micros, TimePoint now);
}
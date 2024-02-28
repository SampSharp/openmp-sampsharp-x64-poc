namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public partial interface ICoreEventHandler : IEventHandler2
{
    void OnTick(Microseconds micros, TimePoint now);
}
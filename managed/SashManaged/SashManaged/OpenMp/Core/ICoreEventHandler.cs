namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface ICoreEventHandler
{
    void OnTick(Microseconds micros, TimePoint now);
}
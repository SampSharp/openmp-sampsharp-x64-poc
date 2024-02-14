using SashManaged.Chrono;

namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface ICoreEventHandler
{
    void OnTick(Microseconds micros, TimePoint now);
}
using SampSharp.OpenMp.Core.Api;
using SampSharp.OpenMp.Core.Chrono;

namespace SampSharp.Entities;

internal class TickingSystem : DisposableSystem, ICoreEventHandler
{
    private ITickingSystem[] _tickers = [];

    [Event]
    public void OnGameModeInit(ISystemRegistry systemRegistry, OpenMp omp)
    {
        var tickers = systemRegistry.Get<ITickingSystem>().ToArray();
        _tickers = new ITickingSystem[tickers.Length];
        Array.Copy(tickers, _tickers, tickers.Length);
        
        AddDisposable(omp.Core.GetEventDispatcher().Add(this));
    }

    public void OnTick(Microseconds micros, TimePoint now)
    {
        for (var i = 0; i < _tickers.Length; i++)
        {
            _tickers[i].Tick();
        }
    }
}
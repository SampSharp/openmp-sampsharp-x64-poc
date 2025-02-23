using SampSharp.OpenMp.Core.Api;
using SampSharp.OpenMp.Core.Chrono;

namespace SampSharp.Entities;

internal class TickingSystem : ISystem, ICoreEventHandler, IDisposable
{
    private ITickingSystem[] _tickers = [];
    private IDisposable? _handler;

    [Event]
    public void OnGameModeInit(ISystemRegistry systemRegistry, OpenMp omp)
    {
        var tickers = systemRegistry.Get<ITickingSystem>().ToArray();
        _tickers = new ITickingSystem[tickers.Length];
        Array.Copy(tickers, _tickers, tickers.Length);
        
        _handler = omp.Core.GetEventDispatcher().Add(this);
    }

    public void OnTick(Microseconds micros, TimePoint now)
    {
        for (var i = 0; i < _tickers.Length; i++)
        {
            _tickers[i].Tick();
        }
    }

    public void Dispose()
    {
        _handler?.Dispose();
        _handler = null;
    }
}
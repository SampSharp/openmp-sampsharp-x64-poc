using SampSharp.Entities;

namespace TestMode.OpenMp.Entities;

public class TestTicker : ITickingSystem
{
    [Event]
    public void OnInitialized()
    {
        Console.WriteLine("On initialized");
    }

    public void Tick()
    {
        // Console.WriteLine("tick");
    }
}
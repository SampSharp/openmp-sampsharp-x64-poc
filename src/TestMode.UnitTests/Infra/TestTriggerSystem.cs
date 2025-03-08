using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace TestMode.UnitTests;

public class TestTriggerSystem : ISystem
{
    [Event]
    public bool OnPlayerCommandText(Player player, string commandText, TestManager testManager)
    {
        if (commandText == "/runtests")
        {
            testManager.Run(TestEnvironment.OnPlayerTrigger);
            player.SendClientMessage("Tests run");
            return true;
        }

        player.SendClientMessage("Did you mean /runtests?");
        return false;
    }

    [Event]
    public void OnGameModeInit(TestManager testManager, ITimerService timerService)
    {
        timerService.Start((sp, timer) =>
        {
            timerService.Stop(timer);

            // after test cases are added to the test suite, this will run them
            testManager.Run(TestEnvironment.OnGameModeInit);

        }, TimeSpan.FromSeconds(1));
    }
}
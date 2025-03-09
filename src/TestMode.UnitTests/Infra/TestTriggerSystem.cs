using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Shouldly;

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
        ShouldlyConfiguration.DefaultFloatingPointTolerance = 0.02f;

        timerService.Delay(_ =>
        {
            var focus = testManager.TestSuites.Any(x => x.TestCases.Any(y => y.Environment.HasFlag(TestEnvironment.Focus)));

            // after test cases are added to the test suite, this will run them
            testManager.Run(focus ? TestEnvironment.Focus | TestEnvironment.OnGameModeInit : TestEnvironment.OnGameModeInit);

        }, TimeSpan.FromSeconds(0.1));
    }
}
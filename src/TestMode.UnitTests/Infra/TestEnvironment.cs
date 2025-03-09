namespace TestMode.UnitTests;

[Flags]
public enum TestEnvironment
{
    Default = 0,
    OnGameModeInit = 1,
    OnPlayerTrigger = 2,
    Focus = 4
}
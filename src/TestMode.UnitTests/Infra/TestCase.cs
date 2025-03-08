namespace TestMode.UnitTests;

public class TestCase
{
    private readonly Action _action;

    public TestCase(string name, TestEnvironment environment, Action action)
    {
        Name = name;
        Environment = environment;
        _action = action;
    }

    public TestStatus Status { get; private set; } = TestStatus.NotRun;
    public string? Error { get; private set; }
    public Exception? Exception { get; private set; }
    public string Name { get; }
    public TestEnvironment Environment { get; }

    public void Skip()
    {
        Status = TestStatus.Skipped;
    }
    public void Run()
    {
        if (Status != TestStatus.NotRun)
        {
            return;
        }

        Status = TestStatus.Running;
        try
        {
            _action();

            Status = TestStatus.Passed;
        }
        catch(Exception ex)
        {
            Status = TestStatus.Failed;

            Error = ex.Message;
            Exception = ex;
        }
    }
}
namespace TestMode.UnitTests;

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    public TestAttribute(TestEnvironment environment)
    {
        Environment = environment;
    }

    public TestAttribute()
    {
    }

    public bool Focus
    {
        get => Environment.HasFlag(TestEnvironment.Focus);
        set => Environment = value ? Environment | TestEnvironment.Focus : Environment & ~TestEnvironment.Focus;
    }

    public string? Name { get; set; }
    public TestEnvironment Environment { get; set; } = TestEnvironment.Default;
}
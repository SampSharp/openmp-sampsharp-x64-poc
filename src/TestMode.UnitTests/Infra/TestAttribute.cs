namespace TestMode.UnitTests;

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    public string? Name { get; set; }
    public TestEnvironment Environment { get; set; } = TestEnvironment.Default;
}
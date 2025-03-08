namespace TestMode.UnitTests;

public record TestSuite(string Name, TestCase[] TestCases, Action? Setup, Action? Cleanup);
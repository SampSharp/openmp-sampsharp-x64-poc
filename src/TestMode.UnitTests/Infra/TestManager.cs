using Microsoft.Extensions.Logging;

namespace TestMode.UnitTests;

public class TestManager(ILogger<TestManager> logger)
{
    public List<TestSuite> TestSuites { get; } = [];

    public void AddTestCase(TestSuite testSuite)
    {
        TestSuites.Add(testSuite);
    }

    public void Run(TestEnvironment environment)
    {
        logger.LogInformation("Running tests...");

        foreach (var testSuite in TestSuites)
        {
            try
            {
                testSuite.Setup?.Invoke();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to setup test suite {name}", testSuite.Name);

                foreach(var testCase in testSuite.TestCases)
                {
                    testCase.Skip();
                }

                continue;
            }

            foreach (var testCase in testSuite.TestCases)
            {
                if (testCase.Status == TestStatus.NotRun && testCase.Environment == environment)
                {
                    logger.LogInformation($"Running test {testCase.Name}...");
                    testCase.Run();
                    logger.LogInformation(testCase.Status.ToString());
                }
            }

            try
            {
                testSuite.Cleanup?.Invoke();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to cleanup test suite {name}", testSuite.Name);
            }
        }

        logger.LogInformation("Tests complete");
        
        var testCases = TestSuites.SelectMany(x => x.TestCases).ToList();

        foreach (var testCase in testCases)
        {
            if(testCase.Status == TestStatus.Failed)
            {
                logger.LogError($"Test {testCase.Name} failed: {testCase.Error}");
            }
        }

        var passed = testCases.Count(x => x.Status == TestStatus.Passed);
        var failed = testCases.Count(x => x.Status == TestStatus.Failed);
        var skipped = testCases.Count(x => x.Status == TestStatus.Skipped);
        var notRun = testCases.Count(x => x.Status == TestStatus.NotRun);

        logger.LogInformation($"Passed: {passed}/{testCases.Count}");
        logger.LogInformation($"Failed: {failed}/{testCases.Count}");
        logger.LogInformation($"Skipped: {skipped}/{testCases.Count}");
        logger.LogInformation($"Not run: {notRun}/{testCases.Count}");
    }
}
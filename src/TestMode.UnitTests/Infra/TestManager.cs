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
        var focus = environment.HasFlag(TestEnvironment.Focus);
        environment &= ~TestEnvironment.Focus;

        logger.LogInformation($"Running tests {environment}...");

        foreach (var testSuite in TestSuites)
        {
            var setup = false;

            foreach (var testCase in testSuite.TestCases)
            {
                var testFocus = (testCase.Environment | testSuite.DefaultEnvironment).HasFlag(TestEnvironment.Focus);
                if (focus && !testFocus)
                {
                    testCase.Skip();
                    continue;
                }

                var testEnv = testCase.Environment == TestEnvironment.Default ? testSuite.DefaultEnvironment : testCase.Environment;
                testEnv &= ~TestEnvironment.Focus;

                if (testCase.Status == TestStatus.NotRun && testEnv == environment)
                {
                    if (!setup)
                    {
                        try
                        {
                            testSuite.Setup?.Invoke();
                            setup = true;
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Failed to setup test suite {name}", testSuite.Name);

                            foreach(var s in testSuite.TestCases)
                            {
                                s.Skip();
                            }

                            goto skipsuite;
                        }
                    }
                    logger.LogDebug($"Running test {testCase.Name}...");
                    testCase.Run();
                    logger.LogDebug(testCase.Status.ToString());
                }
            }

            if (setup)
            {
                try
                {
                    testSuite.Cleanup?.Invoke();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to cleanup test suite {name}", testSuite.Name);
                }
            }

            skipsuite:
            while (false) ;
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
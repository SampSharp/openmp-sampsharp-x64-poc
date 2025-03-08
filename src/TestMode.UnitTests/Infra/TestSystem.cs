using System.Reflection;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace TestMode.UnitTests;

public abstract class TestSystem : ISystem
{
    private IEntityManager? _entityManager;

    public Player Player => _entityManager!.GetComponent<Player>() ?? throw new InvalidOperationException("No player connected");

    public virtual TestEnvironment DefaultEnvironment => TestEnvironment.OnGameModeInit;

    [Event(Name = "OnGameModeInit")]
    public void OnGameModeInit(IEntityManager entityManager, TestManager testManager)
    {
        _entityManager = entityManager;
        
        var suiteName = GetType().Name;
        var testMethods = GetType().GetMethods().Where(m => m.GetCustomAttribute<TestAttribute>() != null);

        var tests = new List<TestCase>();
        foreach (var method in testMethods)
        {
            var testAttribute = method.GetCustomAttribute<TestAttribute>()!;
            var testName = $"{suiteName}.{testAttribute.Name ?? method.Name}";
            var test = new TestCase(testName, testAttribute.Environment, () => method.Invoke(this, BindingFlags.DoNotWrapExceptions, null, null, null));

            tests.Add(test);
        }

        var setup = GetType().GetMethods().FirstOrDefault(x => x.GetCustomAttribute<TestSetup>() != null);
        var cleanup = GetType().GetMethods().FirstOrDefault(x => x.GetCustomAttribute<TestCleanup>() != null);


        testManager.AddTestCase(new TestSuite(
            suiteName, 
            tests.ToArray(), 
            setup == null ? null : () => setup.Invoke(this, BindingFlags.DoNotWrapExceptions, null, null, null),
            cleanup == null ? null : () => cleanup.Invoke(this, BindingFlags.DoNotWrapExceptions, null, null, null), 
            DefaultEnvironment));
    }
}
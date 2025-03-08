using System.Numerics;
using SampSharp.Entities.SAMP;
using Shouldly;

namespace TestMode.UnitTests;

public class ActorTests : TestSystem
{
    private readonly IWorldService _worldService;
    private Actor _actor = null!;

    public ActorTests(IWorldService worldService)
    {
        _worldService = worldService;
    }

    [TestSetup]
    public void CreateTestActor()
    {
        _actor = _worldService.CreateActor(45, new Vector3(4, 5, 6), 45);
    }

    [TestCleanup]
    public void DestroyTestActor()
    {
        _actor?.Destroy();
    }

    [Test]
    public void CreateActor_should_set_properties()
    {
        var actor = _worldService.CreateActor(46, new Vector3(4, 5, 6), 45);
        actor.Skin.ShouldBe(46);
        actor.Position.ShouldBe(new Vector3(4, 5, 6));
        // TODO: actor.Angle.ShouldBe(45);

        actor.Destroy();
    }
    
    [Test]
    public void Actor_Position_roundtrip()
    {
        _actor.Position = new Vector3(1, 2, 3);
        _actor.Position.ShouldBe(new Vector3(1, 2, 3));
    }
    
    [Test]
    public void Health_roundtrip()
    {
        _actor.Health = 94;
        _actor.Health.ShouldBe(94);
    }

    [Test]
    public void IsInvulnerable_roundtrip()
    {
        _actor.IsInvulnerable = true;
        _actor.IsInvulnerable.ShouldBeTrue();
    }

    [Test]
    public void IsInvulnerable_roundtrip_false()
    {
        _actor.IsInvulnerable = false;
        _actor.IsInvulnerable.ShouldBeFalse();
    }

    [Test]
    public void Skin_roundtrip()
    {
        _actor.Skin = 99;
        _actor.Skin.ShouldBe(99);
    }

    [Test]
    public void ApplyAnim()
    {
        _actor.ApplyAnimation("BASEBALL", "Bat_Hit_1", 1, false, false, false, false, 830);
    }

    [Test(Environment = TestEnvironment.OnPlayerTrigger)]
    public void IsStreamedIn()
    {
        _actor.IsStreamedIn(Player);
    }
}
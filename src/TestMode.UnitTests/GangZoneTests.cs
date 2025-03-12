using System.Numerics;
using SampSharp.Entities.SAMP;
using SampSharp.OpenMp.Core.Api;
using Shouldly;

namespace TestMode.UnitTests;

public class GangZoneTests : TestSystem
{
    private readonly IWorldService _worldService;
    private GangZone _gangZone = null!;

    public GangZoneTests(IWorldService worldService)
    {
        _worldService = worldService;
    }

    [TestSetup]
    public void Setup()
    {
        _gangZone = _worldService.CreateGangZone(new Vector2(10, 11), new Vector2(20, 21));
        _gangZone.Color = new Colour(255, 0, 0, 100);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _gangZone?.Destroy();
    }

    [Test]
    public void CreateGangZone_should_set_properties()
    {
        var gangZone = _worldService.CreateGangZone(new Vector2(10, 11), new Vector2(20, 21));

        try
        {
            gangZone.Min.ShouldBe(new Vector2(10, 11));
            gangZone.Max.ShouldBe(new Vector2(20, 21));
        }
        finally
        {
            gangZone.Destroy();
        }
    }
    
    [Test]
    public void Min_should_be_correct()
    {
        _gangZone.Min.ShouldBe(new Vector2(10, 11));
    }

    [Test]
    public void Max_should_be_correct()
    {
        _gangZone.Max.ShouldBe(new Vector2(20, 21));
    }

    [Test]
    public void Color_should_rountrip()
    {
        _gangZone.Color = new Color(1, 2, 3, 4);
        _gangZone.Color.ShouldBe(new Color(1, 2, 3, 4));
    }
    
    [Test(TestEnvironment.OnPlayerTrigger)]
    public void Show_should_work()
    {
        _gangZone.Show();
    }
    [Test(TestEnvironment.OnPlayerTrigger)]
    public void Hide_should_work()
    {
        _gangZone.Hide();
    }
    
    [Test(TestEnvironment.OnPlayerTrigger)]
    public void Show_should_work_for_player()
    {
        _gangZone.Show(Player);
    }

    [Test(TestEnvironment.OnPlayerTrigger)]
    public void Hide_should_work_for_player()
    {
        _gangZone.Show(Player);
        _gangZone.Hide(Player);
    }
    
    [Test(TestEnvironment.OnPlayerTrigger)]
    public void Flash_should_work()
    {
        _gangZone.Flash(Color.White);
    }

    [Test(TestEnvironment.OnPlayerTrigger)]
    public void Flash_should_work_for_player()
    {
        _gangZone.Flash(Player, Color.White);
    }
    
    [Test(TestEnvironment.OnPlayerTrigger)]
    public void StopFlash_should_work()
    {
        _gangZone.Flash(Color.White);
        _gangZone.StopFlash();
    }

    [Test(TestEnvironment.OnPlayerTrigger)]
    public void StopFlash_should_work_for_player()
    {
        _gangZone.Flash(Player, Color.White);
        _gangZone.StopFlash(Player);
    }
}
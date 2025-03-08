using System.Numerics;
using SampSharp.Entities.SAMP;
using Shouldly;

namespace TestMode.UnitTests;

public class GlobalObjectTests : TestSystem
{
    private readonly IWorldService _worldService;
    private GlobalObject _object = null!;

    public GlobalObjectTests(IWorldService worldService)
    {
        _worldService = worldService;
    }

    [TestSetup]
    public void Setup()
    {
        _object = _worldService.CreateObject(100, new Vector3(10, 20, 30), Vector3.Zero);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _object?.Destroy();
    }

    [Test]
    public void CreateObject_should_set_properties()
    {
        var obj = _worldService.CreateObject(100, new Vector3(10, 20, 30), new Vector3(20, 10, 0), 30);
        try
        {
            obj.ModelId.ShouldBe(100);
            obj.Position.ShouldBe(new Vector3(10, 20, 30));
            obj.Rotation.ShouldBe(new Vector3(20, 10, 0));
            obj.DrawDistance.ShouldBe(30);
        }
        finally
        {
            obj.Destroy();
        }
    }

    [Test]
    public void Position_should_roundtrip()
    {
        _object.Position = new Vector3(20, 30, 40);
        _object.Position.ShouldBe(new Vector3(20, 30, 40));
    }

    [Test]
    public void Rotation_should_roundtrip()
    {
        _object.Rotation = new Vector3(20, 30, 40);
        _object.Rotation.ShouldBe(new Vector3(20, 30, 40));
    }
    
    [Test]
    public void Move_and_Stop_should_succeed()
    {
        _object.Move(new Vector3(100, 200, 300), 10, Vector3.Zero);
        _object.Stop();
    }

    [Test]
    public void Move_time_should_be_correct()
    {
        _object.Position = new Vector3(100, 0, 0);
        var result = _object.Move(new Vector3(200, 0, 0), 10, Vector3.Zero);
        _object.Stop();

        result.ShouldBe(10000);
    }

    [Test]
    public void DisableCameraCollisions_should_succeed()
    {
        _object.DisableCameraCollisions();
    }
    
    [Test]
    public void SetMaterial_should_succeed()
    {
        _object.SetMaterial(0, 0, "none", "none", Color.White);
    }

    [Test]
    public void SetMaterialText_should_succeed()
    {
        _object.SetMaterialText(0, "test", ObjectMaterialSize.X128X128, "Arial", 12, true, Color.White, Color.White, ObjectMaterialTextAlign.Center);
    }

    [Test(Environment = TestEnvironment.OnPlayerTrigger)]
    public void AttachToPlayer_should_succeed()
    {
        _object.AttachTo(Player, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }

    [Test]
    public void AttachToVehicle_should_succeed()
    {
        var vehicle = _worldService.CreateVehicle(VehicleModelType.Landstalker, new Vector3(0, 0, 0), 0, 0, 0);

        try
        {
            _object.AttachTo(vehicle, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        }
        finally
        {
            vehicle.Destroy();
        }
    }

    [Test]
    public void AttachToObject_should_succeed()
    {
        var obj = _worldService.CreateObject(100, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        try
        {
            _object.AttachTo(obj, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        }
        finally
        {
            obj.Destroy();
        }
    }
}
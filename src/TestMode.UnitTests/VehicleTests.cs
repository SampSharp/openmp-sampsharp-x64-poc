using System.Numerics;
using SampSharp.Entities.SAMP;
using Shouldly;

namespace TestMode.UnitTests;

public class VehicleTests : TestSystem
{
    private readonly IWorldService _worldService;
    private Vehicle _vehicle = null!;

    public VehicleTests(IWorldService worldService)
    {
        _worldService = worldService;
    }

    [TestSetup]
    public void Setup()
    {
        _vehicle = _worldService.CreateVehicle(VehicleModelType.Landstalker, new Vector3(10, 0, 5), 30, 5, 8);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vehicle?.Destroy();
    }

    [Test]
    public void Position_should_roundtrip()
    {
        _vehicle.Position = new Vector3(1, 2, 3);
        _vehicle.Position.ShouldBe(new Vector3(1, 2, 3));
    }
}
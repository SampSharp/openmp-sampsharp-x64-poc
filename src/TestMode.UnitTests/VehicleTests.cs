using System.Numerics;
using SampSharp.Entities.SAMP;
using SampSharp.OpenMp.Core.Api;
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
    public void CreateVehicle_should_set_properties()
    {
        var vehicle = _worldService.CreateVehicle(VehicleModelType.Landstalker, new Vector3(10, 0, 5), 30, 5, 8);

        try
        {
            vehicle.Model.ShouldBe(VehicleModelType.Landstalker);
            vehicle.Position.ShouldBe(new Vector3(10, 0, 5));
            vehicle.Angle.ShouldBe(30);
            vehicle.Color1.ShouldBe(5);
            vehicle.Color2.ShouldBe(8);
        }
        finally
        {
            vehicle.Destroy();
        }
    }

    [Test]
    public void Position_should_roundtrip()
    {
        _vehicle.Position = new Vector3(1, 2, 3);
        _vehicle.Position.ShouldBe(new Vector3(1, 2, 3));
    }
    
    [Test]
    public void Alarm_should_roundtrip_true()
    {
        _vehicle.Alarm = true;
        _vehicle.Alarm.ShouldBeTrue();
    }

    [Test]
    public void Alarm_should_roundtrip_false()
    {
        _vehicle.Alarm = false;
        _vehicle.Alarm.ShouldBeFalse();
    }

    [Test]
    public void Bonnet_should_roundtrip_true()
    {
        _vehicle.Bonnet = true;
        _vehicle.Bonnet.ShouldBeTrue();
    }
    [Test]
    public void Bonnet_should_roundtrip_false()
    {
        _vehicle.Bonnet = false;
        _vehicle.Bonnet.ShouldBeFalse();
    }
    [Test]
    public void Boot_should_roundtrip_true()
    {
        _vehicle.Boot = true;
        _vehicle.Boot.ShouldBeTrue();
    }
    [Test]
    public void Boot_should_roundtrip_false()
    {
        _vehicle.Boot = false;
        _vehicle.Boot.ShouldBeFalse();
    }
    [Test]
    public void Doors_should_roundtrip_true()
    {
        _vehicle.Doors = true;
        _vehicle.Doors.ShouldBeTrue();
    }
    [Test]
    public void Doors_should_roundtrip_false()
    {
        _vehicle.Doors = false;
        _vehicle.Doors.ShouldBeFalse();
    }
    [Test]
    public void Engine_should_roundtrip_true()
    {
        _vehicle.Engine = true;
        _vehicle.Engine.ShouldBeTrue();
    }
    [Test]
    public void Engine_should_roundtrip_false()
    {
        _vehicle.Engine = false;
        _vehicle.Engine.ShouldBeFalse();
    }
    [Test]
    public void Objective_should_roundtrip_true()
    {
        _vehicle.Objective = true;
        _vehicle.Objective.ShouldBeTrue();
    }
    [Test]
    public void Objective_should_roundtrip_false()
    {
        _vehicle.Objective = false;
        _vehicle.Objective.ShouldBeFalse();
    }
    [Test]
    public void Lights_should_roundtrip_true()
    {
        _vehicle.Lights = true;
        _vehicle.Lights.ShouldBeTrue();
    }
    [Test]
    public void Lights_should_roundtrip_false()
    {
        _vehicle.Lights = false;
        _vehicle.Lights.ShouldBeFalse();
    }
    [Test]
    public void IsBackLeftDoorOpen_should_roundtrip_true()
    {
        _vehicle.IsBackLeftDoorOpen = true;
        _vehicle.IsBackLeftDoorOpen.ShouldBeTrue();
    }
    [Test]
    public void IsBackLeftDoorOpen_should_roundtrip_false()
    {
        _vehicle.IsBackLeftDoorOpen = false;
        _vehicle.IsBackLeftDoorOpen.ShouldBeFalse();
    }
    [Test]
    public void IsBackLeftWindowClosed_should_roundtrip_true()
    {
        _vehicle.IsBackLeftWindowClosed = true;
        _vehicle.IsBackLeftWindowClosed.ShouldBeTrue();
    }
    [Test]
    public void IsBackLeftWindowClosed_should_roundtrip_false()
    {
        _vehicle.IsBackLeftWindowClosed = false;
        _vehicle.IsBackLeftWindowClosed.ShouldBeFalse();
    }
    [Test]
    public void IsBackRightDoorOpen_should_roundtrip_true()
    {
        _vehicle.IsBackRightDoorOpen = true;
        _vehicle.IsBackRightDoorOpen.ShouldBeTrue();
    }
    [Test]
    public void IsBackRightDoorOpen_should_roundtrip_false()
    {
        _vehicle.IsBackRightDoorOpen = false;
        _vehicle.IsBackRightDoorOpen.ShouldBeFalse();
    }
    [Test]
    public void IsBackRightWindowClosed_should_roundtrip_true()
    {
        _vehicle.IsBackRightWindowClosed = true;
        _vehicle.IsBackRightWindowClosed.ShouldBeTrue();
    }
    [Test]
    public void IsBackRightWindowClosed_should_roundtrip_false()
    {
        _vehicle.IsBackRightWindowClosed = false;
        _vehicle.IsBackRightWindowClosed.ShouldBeFalse();
    }
    [Test]
    public void IsDriverDoorOpen_should_roundtrip_true()
    {
        _vehicle.IsDriverDoorOpen = true;
        _vehicle.IsDriverDoorOpen.ShouldBeTrue();
    }
    [Test]
    public void IsDriverDoorOpen_should_roundtrip_false()
    {
        _vehicle.IsDriverDoorOpen = false;
        _vehicle.IsDriverDoorOpen.ShouldBeFalse();
    }
    [Test]
    public void IsDriverWindowClosed_should_roundtrip_true()
    {
        _vehicle.IsDriverWindowClosed = true;
        _vehicle.IsDriverWindowClosed.ShouldBeTrue();
    }
    [Test]
    public void IsDriverWindowClosed_should_roundtrip_false()
    {
        _vehicle.IsDriverWindowClosed = false;
        _vehicle.IsDriverWindowClosed.ShouldBeFalse();
    }
    [Test]
    public void IsPassengerDoorOpen_should_roundtrip_true()
    {
        _vehicle.IsPassengerDoorOpen = true;
        _vehicle.IsPassengerDoorOpen.ShouldBeTrue();
    }
    [Test]
    public void IsPassengerDoorOpen_should_roundtrip_false()
    {
        _vehicle.IsPassengerDoorOpen = false;
        _vehicle.IsPassengerDoorOpen.ShouldBeFalse();
    }
    [Test]
    public void IsPassengerWindowClosed_should_roundtrip_true()
    {
        _vehicle.IsPassengerWindowClosed = true;
        _vehicle.IsPassengerWindowClosed.ShouldBeTrue();
    }
    [Test]
    public void IsPassengerWindowClosed_should_roundtrip_false()
    {
        _vehicle.IsPassengerWindowClosed = false;
        _vehicle.IsPassengerWindowClosed.ShouldBeFalse();
    }

    [Test]
    public void ChangeColor_should_succeed()
    {
        _vehicle.ChangeColor(5, 12);
    }

    [Test]
    public void SetNumberPlate_should_succeed()
    {
        _vehicle.SetNumberPlate("SampSharp");
    }

    [Test]
    public void AddComponent_should_roundtirp()
    {
        _vehicle.AddComponent(1025);
        _vehicle.GetComponentInSlot(CarModType.Wheels).ShouldBe(1025);
    }
    [Test]
    public void ChangePaintjob_should_succeed()
    {
        _vehicle.ChangePaintjob(54);
    }

    [Test]
    public void GetDistanceFromPoint_should_succeed()
    {
        _vehicle.Position = new Vector3(10, 10, 0);
        (_vehicle.GetDistanceFromPoint(new Vector3(20, 20, 0))).ShouldBe(MathF.Sqrt(10 * 10 * 2));
    }

    [Test(TestEnvironment.OnPlayerTrigger)]
    public void IsStreamedIn_should_succeed()
    {
        _vehicle.Position = Vector3.Zero;
        _vehicle.IsStreamedIn(Player).ShouldBeTrue();
    }

    [Test]
    public void LinkToInterior_should_succeed()
    {
        _vehicle.LinkToInterior(4);
        ((IVehicle)_vehicle).GetInterior().ShouldBe(4);// TODO add to api
        _vehicle.LinkToInterior(0);
    }

    [Test]
    public void VirtualWorld_should_roundtrip()
    {
        _vehicle.VirtualWorld = 23;
        _vehicle.VirtualWorld.ShouldBe(23);
        _vehicle.VirtualWorld = 0;
    }

    [Test]
    public void Health_should_roundtrip()
    {
        _vehicle.Health = 876;
        _vehicle.Health.ShouldBe(876);
    }

    [Test]
    public void Repair_should_succeed()
    {
        _vehicle.Health = 500;
        _vehicle.UpdateDamageStatus(13, 14, 15, 16);
        _vehicle.Repair();

        _vehicle.GetDamageStatus(out var panels, out var doors, out var lights, out var tires);
        panels.ShouldBe(0);
        doors.ShouldBe(0);
        lights.ShouldBe(0);
        tires.ShouldBe(0);

        _vehicle.Health.ShouldBe(1000);
    }

    [Test]
    public void UpdateDamageStatus_should_roundtrip()
    {
        _vehicle.UpdateDamageStatus(13, 14, 15, 16);
        _vehicle.GetDamageStatus(out var panels, out var doors, out var lights, out var tires);

        panels.ShouldBe(13);
        doors.ShouldBe(14);
        lights.ShouldBe(15);
        tires.ShouldBe(16);
    }
    
    [Test]
    public void Respawn_should_succeed()
    {
        _vehicle.Respawn();
    }
    
    [Test(TestEnvironment.OnPlayerTrigger)]
    public void SetAngularVelocity_should_succeed()
    {
        // doesn't roundtrip; the setter send a packet to the driver.
        _vehicle.SetAngularVelocity(new Vector3(5, 6, 7));
        
    }

    [Test]
    public void Angle_setter_should_roundtrip()
    {
        _vehicle.Angle = 45.0f;
        _vehicle.Angle.ShouldBe(45);
    }

    [Test]
    public void Model_should_be_correct()
    {
        _vehicle.Model.ShouldBe(VehicleModelType.Landstalker);
    }

    [Test]
    public void HasTrailer_should_be_false_initially()
    {
        _vehicle.HasTrailer.ShouldBeFalse();
    }
    
    [Test(TestEnvironment.OnPlayerTrigger)]
    public void Velocity_setter_should_succeed()
    {
        // doesn't roundtrip; the setter send a packet to the driver.
        Player.PutInVehicle(_vehicle);

        _vehicle.Velocity = new Vector3(1, 1, 1);

        Player.RemoveFromVehicle(true);
    }

    [Test(TestEnvironment.OnPlayerTrigger)]
    public void Velocity_getter_should_succeed()
    {
        // doesn't roundtrip; the setter send a packet to the driver.
        Player.PutInVehicle(_vehicle);

        var result = _vehicle.Velocity;

        result.ShouldBe(Vector3.Zero);

        Player.RemoveFromVehicle(true);
    }

    [Test]
    public void IsSirenOn_should_be_false()
    {
        // state is only set by driver's packets - setter only sets steaming data and does not affect getter
        // setter not available in sampsharp
        _vehicle.IsSirenOn.ShouldBeFalse();
    }

    [Test]
    public void Rotation_should_roundtrip()
    {
        _vehicle.RotationEuler = new Vector3(0, 0, 90);
        _vehicle.RotationEuler.ShouldBe(new Vector3(0, 0, 90));
    }
    [Test]
    public void RemoveComponent_should_succeed()
    {

        _vehicle.AddComponent(1025);
        _vehicle.RemoveComponent(1025);
        _vehicle.GetComponentInSlot(CarModType.Wheels).ShouldBe(0);
    }

}
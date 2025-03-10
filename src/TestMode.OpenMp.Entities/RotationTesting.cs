﻿using System.Numerics;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace TestMode.OpenMp.Entities;

public class RotationTestingSystem(IWorldService worldService, IEntityManager entityManager, IVehicleInfoService vehicleInfoService, ITimerService timerService) : ISystem
{
    [Event]
    public bool OnPlayerCommandText(Player player, string cmdText)
    {
        if (cmdText.StartsWith("/spawn") && cmdText.Length > 7)
        {
            var mdl = int.Parse(cmdText[7..]);
            var vehicle = worldService.CreateVehicle((VehicleModelType)mdl, player.Position + GtaVector.Up * 5, 0, -1, -1);
            player.PutInVehicle(vehicle, 0);
            return true;
        }

        if (cmdText.StartsWith("/coords"))
        {
            var pos = player.Position;
            Console.WriteLine($"Position: {pos}");

            Mark(pos, "*", Color.White);
            Mark(pos + Vector3.UnitX, "+X", Color.Red);
            Mark(pos + Vector3.UnitY, "+Y", Color.Green);
            Mark(pos + Vector3.UnitZ, "+Z", Color.Blue);
            return true;
        }

        if (cmdText == "/circle")
        {
            var center = player.Position + GtaVector.Up;

            Mark(center, "[c]", Color.Red);
            for(var angle = 0; angle < 360; angle += 45)
            {
                var pos = center + Vector3.Transform(GtaVector.Up * 3, Quaternion.CreateFromAxisAngle(GtaVector.Up, float.DegreesToRadians(angle)));
            
                Mark(pos, $"[{angle}]", Color.Blue);
            }

            return true;
        }

        if (cmdText == "/angle")
        {
            var v= player.Vehicle;

            if (v == null)
            {
                return false;
            }

            var zAngle = v.Angle;

            var mat = Matrix4x4.CreateFromQuaternion(v.RotationQuaternion);
            var zAngle2 = float.RadiansToDegrees(MathHelper.GetZAngleFromRotationMatrix(mat));

            player.SendClientMessage($"Vehicle Z-angle(open.mp): {zAngle}, ZAngle through RotQuat(s#): {zAngle2}");
            
            return true;
        }
        return false;
    }


    [Timer(100)]
    public void UpdateMark()
    {
        foreach (var vehicle in entityManager.GetComponents<Vehicle>())
        {
            var label = vehicle.GetComponentInChildren<TextLabel>() 
                      ?? worldService.CreateTextLabel("[x]", Color.White, Vector3.Zero, 20, parent: vehicle);

            // calculate offset to the rear center bumper of the vehicle
            var model = vehicle.Model;
            var offset = vehicleInfoService.GetModelInfo(model, VehicleModelInfoType.PetrolCap);
 
            var rotMatrix = Matrix4x4.CreateFromQuaternion(vehicle.RotationQuaternion);
            var trMatrix = Matrix4x4.CreateTranslation(offset) * rotMatrix * Matrix4x4.CreateTranslation(vehicle.Position);

            var point = trMatrix.Translation;
     
            label.Position = point;
        }
    }

    [Event]
    public bool OnUnoccupiedVehicleUpdate(Vehicle veh, Player player, Vector3 position, Vector3 velo, int seat)
    {
        // TODO: broken rn. don't return false by default when no event implementation is available.
        return true;
    }
    
    private void Mark(Vector3 point, string txt, Color color)
    {
        var label = worldService.CreateTextLabel(txt, color, point, 100, 0, false);
        timerService.Delay(_ => entityManager.Destroy(label), TimeSpan.FromSeconds(10));
    }
}
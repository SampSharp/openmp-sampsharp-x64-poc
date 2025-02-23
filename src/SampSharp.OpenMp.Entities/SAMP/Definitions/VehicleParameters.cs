using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

[StructLayout(LayoutKind.Sequential)]
public record struct VehicleParameters(
    VehicleParameterValue Engine,
    VehicleParameterValue Lights,
    VehicleParameterValue Alarm,
    VehicleParameterValue Doors,
    VehicleParameterValue Bonnet,
    VehicleParameterValue Boot,
    VehicleParameterValue Objective,
    VehicleParameterValue Siren,
    VehicleParameterValue DoorDriver,
    VehicleParameterValue DoorPassenger,
    VehicleParameterValue DoorBackLeft,
    VehicleParameterValue DoorBackRight,
    VehicleParameterValue WindowDriver,
    VehicleParameterValue WindowPassenger,
    VehicleParameterValue WindowBackLeft,
    VehicleParameterValue WindowBackRight)
{
    internal static VehicleParameters FromParams(ref VehicleParams value)
    {
        return Unsafe.As<VehicleParams, VehicleParameters>(ref value);
    }

    [Pure]
    internal VehicleParams ToParams()
    {
        return Unsafe.As<VehicleParameters, VehicleParams>(ref this);
    }
}
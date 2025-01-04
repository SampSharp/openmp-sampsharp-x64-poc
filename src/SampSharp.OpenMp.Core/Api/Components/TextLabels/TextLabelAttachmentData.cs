using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TextLabelAttachmentData
{
    public readonly int PlayerId; // default: INVALID_PLAYER_ID
    public readonly int VehicleId; // default: INVALID_VEHICLE_ID
}
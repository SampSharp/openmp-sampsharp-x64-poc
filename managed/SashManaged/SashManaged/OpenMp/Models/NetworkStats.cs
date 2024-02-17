using System.Runtime.InteropServices;

namespace SashManaged.OpenMp.Models;

[StructLayout(LayoutKind.Sequential)]
public readonly struct NetworkStats
{
    public readonly uint ConnectionStartTime;
    public readonly uint MessageSendBuffer;
    public readonly uint MessagesSent;
    public readonly uint TotalBytesSent;
    public readonly uint AcknowlegementsSent;
    public readonly uint AcknowlegementsPending;
    public readonly uint MessagesOnResendQueue;
    public readonly uint MessageResends;
    public readonly uint MessagesTotalBytesResent;
    public readonly float Packetloss;
    public readonly uint MessagesReceived;
    public readonly uint MessagesReceivedPerSecond;
    public readonly uint BytesReceived;
    public readonly uint AcknowlegementsReceived;
    public readonly uint DuplicateAcknowlegementsReceived;
    public readonly double BitsPerSecond;
    public readonly double BpsSent;
    public readonly double BpsReceived;
    public readonly bool IsActive;
    public readonly int ConnectMode;
    public readonly uint ConnectionElapsedTime;
}
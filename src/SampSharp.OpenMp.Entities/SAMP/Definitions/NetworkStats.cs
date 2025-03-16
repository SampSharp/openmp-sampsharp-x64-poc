namespace SampSharp.Entities.SAMP;

public sealed class NetworkStats
{
    private readonly SampSharp.OpenMp.Core.Api.NetworkStats _stats;

    public NetworkStats(SampSharp.OpenMp.Core.Api.NetworkStats stats)
    {
        _stats = stats;
    }

    public int ConnectionStartTime => (int)_stats.ConnectionStartTime;
    public int MessageSendBuffer => (int)_stats.MessageSendBuffer;

    public int MessagesSent => (int)_stats.MessagesSent;
    public int TotalBytesSent => (int)_stats.TotalBytesSent;
    public int AcknowlegementsSent => (int)_stats.AcknowlegementsSent;
    public int AcknowlegementsPending => (int)_stats.AcknowlegementsPending;
    public int MessagesOnResendQueue => (int)_stats.MessagesOnResendQueue;
    public int MessageResends => (int)_stats.MessageResends;
    public int MessagesTotalBytesResent => (int)_stats.MessagesTotalBytesResent;
    public float Packetloss => _stats.Packetloss;
    public int MessagesReceived => (int)_stats.MessagesReceived;
    public int MessagesReceivedPerSecond => (int)_stats.MessagesReceivedPerSecond;
    public int BytesReceived => (int)_stats.BytesReceived;
    public int AcknowlegementsReceived => (int)_stats.AcknowlegementsReceived;
    public int DuplicateAcknowlegementsReceived => (int)_stats.DuplicateAcknowlegementsReceived;
    public double BitsPerSecond => _stats.BitsPerSecond;
    public double BpsSent => _stats.BpsSent;
    public double BpsReceived => _stats.BpsReceived;
    public bool IsActive => _stats.IsActive;
    public int ConnectMode => _stats.ConnectMode;
    public uint ConnectionElapsedTime => _stats.ConnectionElapsedTime;
}

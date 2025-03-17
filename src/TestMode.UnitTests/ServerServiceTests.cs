using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities.SAMP;
using SampSharp.OpenMp.Core.Api;
using Xunit;

namespace TestMode.UnitTests;

public class ServerServiceTests : TestBase
{
    [Fact]
    public void BlockIpAddress_should_succeed()
    {
        var ssz = Marshal.SizeOf<BanEntryMarshaller.Native>();
        var sut = Services.GetRequiredService<IServerService>();

        sut.BlockIpAddress("127.0.0.1");
    }
    [Fact]
    public void UnblockIpAddress_should_succeed()
    {
        var sut = Services.GetRequiredService<IServerService>();

        sut.UnBlockIpAddress("127.0.0.1");
    }
}

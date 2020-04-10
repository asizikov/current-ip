using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using CurrentIp.Web.Providers;
using Shouldly;
using Xunit;

namespace CurrentIp.Web.UnitTests.Providers {
  public class RdpFileProviderTests {
    private readonly IRdpFileProvider _provider;

    public RdpFileProviderTests() {
      _provider = new RdpFileProvider();
    }

    [Fact]
    private async Task FileContent_AsExpected() {
      var record = new IpRecord {CurrentIP = "127.0.0.1"};

      await using var stream = await _provider.CreateFileStreamAsync(record, CancellationToken.None);
      stream.Seek(0, SeekOrigin.Begin);
      using var reader = new StreamReader( stream );
      var firstLine = await reader.ReadLineAsync();
      firstLine.ShouldBe($"full address:s:{record.CurrentIP}:3389");
    }
  }
}
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Web.Providers {
  public class RdpFileProvider : IRdpFileProvider {
    public async Task<Stream> CreateFileStreamAsync(IpRecord record, CancellationToken token) {
      var sb = new StringBuilder();
      sb.AppendLine($"full address:s:{record.CurrentIP}:3389");
      sb.AppendLine("prompt for credentials:i:1");
      sb.AppendLine("administrative session:i:1");
      var content = sb.ToString();
      var bytes = Encoding.ASCII.GetBytes(content);
      var memoryStream  = new MemoryStream();
      await memoryStream.WriteAsync(bytes, token).ConfigureAwait(false);
      return memoryStream;
    }
  }
}
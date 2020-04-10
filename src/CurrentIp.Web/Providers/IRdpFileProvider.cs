using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Web.Providers {
  public interface IRdpFileProvider {
    Task<Stream> CreateFileStreamAsync(IpRecord record, CancellationToken token);
  }
}
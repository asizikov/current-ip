using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Storage {
  public interface IMachineHistoryStorage {
    Task<int> AddRecordAsync(IpRecord record, CancellationToken token);
    Task<IEnumerable<IpRecord>> GetHistoryAsync(CancellationToken token);
    Task<IpRecord> GetLatestAsync(CancellationToken token);
  }
}
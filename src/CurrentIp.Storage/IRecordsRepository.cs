using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Storage {
  public interface IRecordsRepository {
    Task<IpRecord> GetLatestAsync(CancellationToken token);
    Task<IEnumerable<IpRecord>> GetHistoryAsync(int maxItems, CancellationToken token);
    Task<IpRecord> CreateAsync(Report record, CancellationToken token);
  }
}
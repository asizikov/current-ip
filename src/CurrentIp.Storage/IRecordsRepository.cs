using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Storage {
  public interface IRecordsRepository {
    Task<IpRecord> GetLatestAsync(string machineTag, CancellationToken token);
    Task<IEnumerable<IpRecord>> GetHistoryAsync(string machineTag, CancellationToken token);
    Task<IpRecord> CreateAsync(Report record, CancellationToken token);
  }
}
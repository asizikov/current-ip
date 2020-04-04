using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Storage.Local {
  public class InMemoryRecordsRepository : IRecordsRepository {
    private readonly List<IpRecord> _records;

    public InMemoryRecordsRepository() {
      _records = new List<IpRecord>();
    }

    public Task<IpRecord> GetLatestAsync(CancellationToken token) {
      if (_records.Count == 0) {
        return Task.FromResult<IpRecord>(null);
      }

      return Task.FromResult(_records[^1]);
    }

    public Task<IEnumerable<IpRecord>> GetHistoryAsync(int maxItems, CancellationToken token) {
      throw new System.NotImplementedException();
    }

    public Task<IpRecord> CreateAsync(Report record, CancellationToken token) {
      var ipRecord = new IpRecord {
        CurrentIP = record.CurrentIP,
        MachineName = record.MachineName,
        LastSeen = DateTime.Now
      };
      _records.Add(ipRecord);
      return Task.FromResult(ipRecord);
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Storage.Local {
  public class InMemoryMachineHistoryStorage {
    private readonly IpRecord[] _records;
    private int _tail;

    public InMemoryMachineHistoryStorage() {
      _records = new IpRecord[StorageConfiguration.NumberOfRecordsPerMachine];
      _tail = -1;
    }

    public Task<int> AddRecordAsync(IpRecord record, CancellationToken token) {
      _tail = (_tail + 1) % _records.Length;
      _records[_tail] = record;
      return Task.FromResult(_tail);
    }

    public Task<IEnumerable<IpRecord>> GetHistoryAsync(CancellationToken token) {
      if (_tail == -1) {
        return Task.FromResult(Enumerable.Empty<IpRecord>());
      }

      var result = new List<IpRecord>();
      for (var i = 0; i < _records.Length; i++) {
        var index = Math.Abs(_tail - i) % _records.Length;
        var next = _records[index];
        if (_records[index] is null) {
          continue;
        }
        result.Add(next);
      }
      return Task.FromResult((IEnumerable<IpRecord>) result);
    }

    public Task<IpRecord> GetLatestAsync(in CancellationToken token) {
      var latest = _tail == -1 ? null : _records[_tail];
      return Task.FromResult(latest);
    }
  }
}
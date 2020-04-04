using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Storage.Local {
  public class InMemoryRecordsRepository : IRecordsRepository {
    private readonly Dictionary<string, InMemoryMachineHistoryStorage> _records;

    public InMemoryRecordsRepository() {
      _records = new Dictionary<string, InMemoryMachineHistoryStorage>();
    }

    public async Task<IpRecord> GetLatestAsync(string machineName, CancellationToken token) {
      var machineNameKey = machineName.ToLowerInvariant();
      if (!_records.ContainsKey(machineNameKey)) {
        return null;
      }

      var latest = await _records[machineNameKey].GetLatestAsync(token).ConfigureAwait(false);
      return latest;
    }

    public Task<IEnumerable<IpRecord>> GetHistoryAsync(string machineTag, CancellationToken token) {
      var machineTagKey = machineTag.ToLowerInvariant();
      if (_records.ContainsKey(machineTagKey)) {
        return Task.FromResult(Enumerable.Empty<IpRecord>());
      }

      return _records[machineTagKey].GetHistoryAsync(token);
    }

    public async Task<IpRecord> CreateAsync(Report record, CancellationToken token) {
      var ipRecord = new IpRecord {
        CurrentIP = record.CurrentIP,
        MachineName = record.MachineName,
        LastSeen = DateTime.Now
      };
      var machineNameKey = record.MachineTag.ToLowerInvariant();
      if (!_records.ContainsKey(machineNameKey)) {
        _records[machineNameKey] = new InMemoryMachineHistoryStorage();
      }

      _ = await _records[machineNameKey].AddRecordAsync(ipRecord, token).ConfigureAwait(false);
      return ipRecord;
    }
  }
}
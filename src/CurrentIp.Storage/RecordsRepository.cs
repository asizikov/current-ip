using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Storage {
  public class RecordsRepository : IRecordsRepository {
    private readonly IMachineHistoryStorageProvider _historyStorageProvider;

    public RecordsRepository(IMachineHistoryStorageProvider machineHistoryStorageProvider) {
      _historyStorageProvider = machineHistoryStorageProvider;
    }

    public async Task<IpRecord> GetLatestAsync(string machineTag, CancellationToken token) {
      var machineTagKey = machineTag.ToLowerInvariant();
      var machineHistoryStorage = await _historyStorageProvider.GetForMachineAsync(machineTagKey, token).ConfigureAwait(false);
      var latest = await machineHistoryStorage.GetLatestAsync(token).ConfigureAwait(false);
      return latest;
    }

    public async Task<IEnumerable<IpRecord>> GetHistoryAsync(string machineTag, CancellationToken token) {
      var machineTagKey = machineTag.ToLowerInvariant();
      var machineHistoryStorage = await _historyStorageProvider.GetForMachineAsync(machineTagKey, token).ConfigureAwait(false);

      return await machineHistoryStorage.GetHistoryAsync(token).ConfigureAwait(false);
    }

    public async Task<IpRecord> CreateAsync(Report record, CancellationToken token) {
      var ipRecord = new IpRecord {
        CurrentIP = record.CurrentIP,
        MachineName = record.MachineName,
        LastSeen = DateTime.Now
      };
      var machineTagKey = record.MachineTag.ToLowerInvariant();
      var machineHistoryStorage = await _historyStorageProvider.GetForMachineAsync(machineTagKey, token).ConfigureAwait(false);
      _ = await machineHistoryStorage.AddRecordAsync(ipRecord, token).ConfigureAwait(false);
      return ipRecord;
    }
  }
}
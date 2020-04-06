using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CurrentIp.Storage {
  public class MachineHistoryStorage : IMachineHistoryStorage {
    private readonly string _machineRootKey;
    private readonly IDistributedCache _distributedCache;

    public MachineHistoryStorage(string machineRootKey, IDistributedCache distributedCache) {
      _machineRootKey = machineRootKey;
      _distributedCache = distributedCache;
    }

    public async Task<int> AddRecordAsync(IpRecord record, CancellationToken token) {
      var page = await GetPageAsync(token).ConfigureAwait(false);
      page.Tail = (page.Tail + 1) % page.History.Length;
      page.History[page.Tail] = record;
      await SavePageAsync(page, token).ConfigureAwait(false);
      return page.Tail;
    }

    private async Task<MachineHistoryPage> GetPageAsync(CancellationToken token) {
      var json = await _distributedCache.GetStringAsync(_machineRootKey, token).ConfigureAwait(false);
      return JsonConvert.DeserializeObject<MachineHistoryPage>(json);
    }

    private async Task SavePageAsync(MachineHistoryPage page, CancellationToken token) {
      await _distributedCache.SetStringAsync(_machineRootKey, JsonConvert.SerializeObject(page), token).ConfigureAwait(false);
    }

    public async Task<IEnumerable<IpRecord>> GetHistoryAsync(CancellationToken token) {
      var page = await GetPageAsync(token).ConfigureAwait(false);
      if (page.Tail == -1) {
        return Enumerable.Empty<IpRecord>();
      }

      var result = new List<IpRecord>();
      for (var i = 0; i < page.History.Length; i++) {
        var index = Math.Abs(page.Tail - i) % page.History.Length;
        var next = page.History[index];
        if (page.History[index] is null) {
          continue;
        }

        result.Add(next);
      }

      return result;
    }

    public async Task<IpRecord> GetLatestAsync(CancellationToken token) {
      var page = await GetPageAsync(token).ConfigureAwait(false);
      var latest = page.Tail == -1 ? null : page.History[page.Tail];
      return latest;
    }
  }
}
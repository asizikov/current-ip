using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CurrentIp.Storage {
  public class MachineHistoryStorageProvider : IMachineHistoryStorageProvider {
    private const string StorageKeyPrefix = "MachineHistoryStorage";
    private readonly IDistributedCache _distributedCache;

    public MachineHistoryStorageProvider(IDistributedCache distributedCache) {
      _distributedCache = distributedCache;
    }

    public async Task<IMachineHistoryStorage> GetForMachineAsync(string machineNameKey, CancellationToken token) {
      var machineRootKey = $"{StorageKeyPrefix}.{machineNameKey}";
      var value = await _distributedCache.GetAsync(machineRootKey, token).ConfigureAwait(false);
      if (value is null) {
        await _distributedCache.SetStringAsync(machineRootKey, JsonConvert.SerializeObject(MachineHistoryPage.Empty(machineNameKey)), token)
          .ConfigureAwait(false);
      }

      return new MachineHistoryStorage(machineRootKey, _distributedCache);
    }
  }
}
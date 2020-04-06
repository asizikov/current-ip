using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace CurrentIp.Storage.UnitTests {
  public class MachineHistoryStorageProviderTests {
    private readonly IMachineHistoryStorageProvider _machineHistoryStorageProvider;
    private readonly CancellationToken _token;
    private readonly IDistributedCache _memoryDistributedCache;

    public MachineHistoryStorageProviderTests() {
      _memoryDistributedCache = new MemoryDistributedCache(new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions()));
      _machineHistoryStorageProvider = new MachineHistoryStorageProvider(_memoryDistributedCache);
      _token = CancellationToken.None;
    }

    [Fact]
    public async Task GetForMachine_Always_Returns_Storage_And_Inits_Cache() {
      var machineHistoryStorage = await _machineHistoryStorageProvider.GetForMachineAsync("key", _token);
      machineHistoryStorage.ShouldNotBeNull();
      var bytes = await _memoryDistributedCache.GetAsync("MachineHistoryStorage.key", _token);
      bytes.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetForMachine_Returns_New_Storage_Every_Time() {
      var set = new HashSet<IMachineHistoryStorage>();
      for (var i = 0; i < 5; i++) {
        var machineHistoryStorage = await _machineHistoryStorageProvider.GetForMachineAsync("key", _token);
        set.Contains(machineHistoryStorage).ShouldBeFalse();
        set.Add(machineHistoryStorage);
      }
    }
  }
}
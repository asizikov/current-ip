using System;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace CurrentIp.Storage.UnitTests {
  public class InMemoryMachineHistoryStorageTests {
    private readonly MachineHistoryStorage _storage;
    private readonly CancellationToken _token;
    private readonly IDistributedCache _memoryDistributedCache;
    private const string rootKey = "rootkey";

    public InMemoryMachineHistoryStorageTests() {
      _memoryDistributedCache = new MemoryDistributedCache(new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions()));
      _storage = new MachineHistoryStorage(rootKey, _memoryDistributedCache);
      _token = CancellationToken.None;
    }

    [Fact]
    public async Task RecordNumber_Increased() {
      await _memoryDistributedCache.SetStringAsync(rootKey, JsonConvert.SerializeObject(MachineHistoryPage.Empty("name")), _token);
      var recordNumber = await _storage.AddRecordAsync(new IpRecord(), _token);
      recordNumber.ShouldBe(0);
    }

    [Fact]
    public async Task RecordNumber_Does_Not_Grow_Over_Max_Val() {
      await _memoryDistributedCache.SetStringAsync(rootKey, JsonConvert.SerializeObject(MachineHistoryPage.Empty("name")), _token);
      for (var i = 0; i < StorageConfiguration.NumberOfRecordsPerMachine * 2; i++) {
        var recordNumber = await _storage.AddRecordAsync(new IpRecord(), _token);
        recordNumber.ShouldBe(i % StorageConfiguration.NumberOfRecordsPerMachine);
      }
    }

    [Fact]
    public async Task Records_Returned_In_ReversedOrder() {
      await _memoryDistributedCache.SetStringAsync(rootKey, JsonConvert.SerializeObject(MachineHistoryPage.Empty("name")), _token);
      for (var i = 0; i < StorageConfiguration.NumberOfRecordsPerMachine; i++) {
        _ = await _storage.AddRecordAsync(new IpRecord {
          CurrentIP = i.ToString()
        }, _token);
      }

      var records = await _storage.GetHistoryAsync(_token);
      using var enumerator = records.GetEnumerator();
      enumerator.MoveNext();
      for (var i = 0; i < StorageConfiguration.NumberOfRecordsPerMachine; i++) {
        var next = enumerator.Current;
        next.ShouldSatisfyAllConditions(
          () => next.ShouldNotBeNull(),
          () => next.CurrentIP.ShouldBe((StorageConfiguration.NumberOfRecordsPerMachine - i - 1).ToString())
        );
        enumerator.MoveNext();
      }
    }
  }
}
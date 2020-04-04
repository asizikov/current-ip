using System;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using CurrentIp.Storage.Local;
using Shouldly;
using Xunit;

namespace CurrentIp.Storage.UnitTests {
  public class InMemoryMachineHistoryStorageTests {
    private readonly InMemoryMachineHistoryStorage _storage;
    private readonly CancellationToken _token;

    public InMemoryMachineHistoryStorageTests() {
      _storage = new InMemoryMachineHistoryStorage();
      _token = CancellationToken.None;
    }

    [Fact]
    public async Task RecordNumber_Increased() {
      var recordNumber = await _storage.AddRecordAsync(new IpRecord(), _token);
      recordNumber.ShouldBe(0);
    }

    [Fact]
    public async Task RecordNumber_Does_Not_Grow_Over_Max_Val() {
      for (var i = 0; i < StorageConfiguration.NumberOfRecordsPerMachine * 2; i++) {
        var recordNumber = await _storage.AddRecordAsync(new IpRecord(), _token);
        recordNumber.ShouldBe(i % StorageConfiguration.NumberOfRecordsPerMachine);
      }
    }

    [Fact]
    public async Task Records_Returned_In_ReversedOrder() {
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
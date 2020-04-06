using System.Threading;
using System.Threading.Tasks;

namespace CurrentIp.Storage {
  public interface IMachineHistoryStorageProvider {
    Task<IMachineHistoryStorage> GetForMachineAsync(string machineNameKey, CancellationToken token);
  }
}
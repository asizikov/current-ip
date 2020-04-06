using CurrentIp.DataModel;

namespace CurrentIp.Storage {
  public class MachineHistoryPage {
    public string Tag { get; set; }
    public int Tail { get; set; }
    public IpRecord[] History { get; set; }

    public static MachineHistoryPage Empty(string machineTag) =>
      new MachineHistoryPage {
        History = new IpRecord[StorageConfiguration.NumberOfRecordsPerMachine],
        Tag = machineTag,
        Tail = -1
      };
  }
}
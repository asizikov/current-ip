using System;

namespace CurrentIp.DataModel {
  public class IpRecord {
    public string MachineName { get; set; }
    public string CurrentIP { get; set; }
    public DateTime LastSeen { get; set; }
  }
}
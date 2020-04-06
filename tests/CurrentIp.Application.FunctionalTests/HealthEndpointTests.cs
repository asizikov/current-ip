using System;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.ApiClient;
using CurrentIp.DataModel;
using Shouldly;
using Xunit;

namespace CurrentIp.Application.FunctionalTests {
  public class HealthEndpointTests {
    private string _baseUrl = "http://127.0.0.1:8080/";
    private readonly CurrentIpApiClient _currentIpApiClient;

    public HealthEndpointTests() {
      _currentIpApiClient = new CurrentIpApiClient(new Uri(_baseUrl));
    }

    [Fact]
    public async Task Health_Check_Returns_Ok() {
      var isHealthy = await _currentIpApiClient.IsHealthyAsync(CancellationToken.None);
      isHealthy.ShouldBeTrue();
    }

    [Fact]
    public async Task Post_Report_Returns_Ok() {
      var report = new Report {
        CurrentIP = "10.10.10.1",
        MachineName = "abadsb/abab",
        MachineTag = "machine-tag"
      };
      var result = await _currentIpApiClient.SubmitReportAsync(report, CancellationToken.None);
      result.ShouldBeTrue();
    }
  }
}
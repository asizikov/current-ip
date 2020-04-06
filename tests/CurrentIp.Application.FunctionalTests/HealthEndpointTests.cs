using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace CurrentIp.Application.FunctionalTests {
  public class HealthEndpointTests {
    [Fact]
    public async Task Health_Check_Returns_Ok() {
      var baseUrl = "http://127.0.0.1:8080/";
      var httpClient = new HttpClient {BaseAddress = new Uri(baseUrl)};
      var httpResponseMessage = await httpClient.GetAsync("api/health");
      httpResponseMessage.ShouldSatisfyAllConditions(
        () => httpResponseMessage.ShouldNotBeNull(),
        () => httpResponseMessage.IsSuccessStatusCode.ShouldBeTrue()
      );
    }

    [Fact]
    public async Task Post_Report_Returns_Ok() {
      var baseUrl = "http://127.0.0.1:8080/";
      var httpClient = new HttpClient {BaseAddress = new Uri(baseUrl)};
      var httpResponseMessage = await httpClient.PostAsync("api/currentip/report", new StringContent(JsonConvert.SerializeObject(new Report {
        CurrentIP = "10.10.10.1",
        MachineName = "abadsb/abab",
        MachineTag = "machine-tag"
      }), Encoding.Default, "application/json"));
      httpResponseMessage.ShouldSatisfyAllConditions(
        () => httpResponseMessage.ShouldNotBeNull(),
        () => httpResponseMessage.IsSuccessStatusCode.ShouldBeTrue()
      );
    }
  }
}
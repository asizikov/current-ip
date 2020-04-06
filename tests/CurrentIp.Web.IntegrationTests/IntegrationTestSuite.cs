using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace CurrentIp.Web.IntegrationTests {
  public class IntegrationTestSuite {
    private readonly TestServer _server;
    private readonly HttpClient _client;

    public IntegrationTestSuite() {
      var builder = new WebHostBuilder()
        .UseStartup<Startup>()
        .UseEnvironment("IntegrationTests");
      _server = new TestServer(builder);
      _client = _server.CreateClient();
    }

    [Fact]
    private async Task Health() {
      var httpResponseMessage = await _client.GetAsync("/api/health").ConfigureAwait(false);
      httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("/api/currentip/whatever/latest")]
    [InlineData("/api/currentip/whatever/history")]
    private async Task GetRequests(string path) {
      var httpResponseMessage = await _client.GetAsync(path).ConfigureAwait(false);
      httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    private async Task Post_Get_Scenario() {
      var responseMessage = await _client.PostAsync("/api/currentip/report",
        new StringContent(JsonConvert.SerializeObject(new Report {
          CurrentIP = "10.10.10.1",
          MachineName = "abadsb/abab",
          MachineTag = "machine-tag"
        }), Encoding.Default, "application/json"));
      responseMessage.StatusCode.ShouldBe(HttpStatusCode.Created);
      var httpResponseMessage = await _client.GetAsync("/api/currentip/machine-tag/latest").ConfigureAwait(false);
      httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
  }
}
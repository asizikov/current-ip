using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace CurrentIp.Application.FunctionalTests {
  public class HealthEndpointTests {
    [Fact]
    public async Task Health_Check_Returns_Ok() {
      var baseUrl = "http://localhost:5000/";
      var httpClient = new HttpClient {BaseAddress = new Uri(baseUrl)};
      var httpResponseMessage = await httpClient.GetAsync("api/health");
      httpResponseMessage.ShouldSatisfyAllConditions(
        () => httpResponseMessage.ShouldNotBeNull(),
        () => httpResponseMessage.IsSuccessStatusCode.ShouldBeTrue()
      );
    }
  }
}
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using Newtonsoft.Json;

namespace CurrentIp.ApiClient {
  public class CurrentIpApiClient {
    private readonly Uri _baseUri;
    private readonly HttpClient _httpClient;

    public CurrentIpApiClient(Uri baseUri) {
      _baseUri = baseUri;
      _httpClient = new HttpClient {BaseAddress = _baseUri};
    }

    public async Task<bool> IsHealthyAsync(CancellationToken token) {
      var httpResponseMessage = await _httpClient.GetAsync("api/health", token).ConfigureAwait(false);
      return httpResponseMessage.IsSuccessStatusCode;
    }

    public async Task<bool> SubmitReportAsync(Report report, CancellationToken token) {
      var httpResponseMessage = await _httpClient.PostAsync("api/currentip/report",
        new StringContent(JsonConvert.SerializeObject(report), Encoding.Default, Constants.MediaType), token);
      return httpResponseMessage.IsSuccessStatusCode;
    }
  }
}
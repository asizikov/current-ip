using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.ApiClient;
using CurrentIp.DataModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrentIp.Service {
  public class Worker : BackgroundService {
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger) {
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
      while (!stoppingToken.IsCancellationRequested) {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        var serviceClient = new CurrentIpApiClient(new Uri("https://app-current-ip-api-prod.azurewebsites.net/"));
        var ipAddress = await GetPublicIPAsync(stoppingToken).ConfigureAwait(false);
        var report = new Report {
          CurrentIP = ipAddress,
          MachineName = Environment.MachineName,
          MachineTag = "dev-win-10"
        };
        _logger.LogInformation($"Going to log ip {report.CurrentIP} for {report.MachineName} (tagged as {report.MachineTag})");
        await serviceClient.SubmitReportAsync(report, stoppingToken).ConfigureAwait(false);
        await Task.Delay(TimeSpan.FromSeconds(60 * 5), stoppingToken);
      }
    }

    public async Task<string> GetPublicIPAsync(CancellationToken token) {
      var request = WebRequest.Create("https://api.ipify.org/");
      using var response = await request.GetResponseAsync();
      using var stream = new StreamReader(response.GetResponseStream());
      return await stream.ReadToEndAsync();
    }
  }
}
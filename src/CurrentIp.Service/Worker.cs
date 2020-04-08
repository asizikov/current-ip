using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.ApiClient;
using CurrentIp.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrentIp.Service {
  public class Worker : BackgroundService {
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _serviceBaseUrl;
    private readonly string _serviceBasePort;
    private readonly int _timeOut;

    public Worker(ILogger<Worker> logger, IConfiguration configuration) {
      _logger = logger;
      _configuration = configuration;
      _serviceBaseUrl = _configuration.GetSection("CURRENT_IP_SERVICE_HOST_URL").Value;
      _serviceBasePort = _configuration.GetSection("CURRENT_IP_SERVICE_HOST_PORT").Value;
      logger.LogInformation($"found host uri: {_serviceBaseUrl}:{_serviceBasePort}");
      var value = _configuration.GetSection("CURRENT_IP_SERVICE_TIMEOUT_SECONDS").Value;
      _timeOut = Int32.Parse(value);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
      while (!stoppingToken.IsCancellationRequested) {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        _logger.LogInformation($"API found on {_serviceBaseUrl}");
        var baseUri = new Uri($"http://{_serviceBaseUrl}:{_serviceBasePort}");
        var serviceClient = new CurrentIpApiClient(baseUri);
        var ipAddress = await GetPublicIPAsync(stoppingToken).ConfigureAwait(false);
        var report = new Report {
          CurrentIP = ipAddress,
          MachineName = Environment.MachineName,
          MachineTag = "dev-win-10"
        };
        _logger.LogInformation($"Going to log ip {report.CurrentIP} for {report.MachineName} (tagged as {report.MachineTag}) to {baseUri}");
        try {
          var status = await serviceClient.SubmitReportAsync(report, stoppingToken).ConfigureAwait(false);
          _logger.LogInformation($"Submitted report to {baseUri} with success status {status}");
        }
        catch (Exception e) {
          _logger.LogError(e,"Failed to publish a report");
        }
        await Task.Delay(TimeSpan.FromSeconds(_timeOut), stoppingToken);
      }
    }

    private static async Task<string> GetPublicIPAsync(CancellationToken token) {
      var request = WebRequest.Create("https://api.ipify.org/");
      using var response = await request.GetResponseAsync();
      using var stream = new StreamReader(response.GetResponseStream());
      return await stream.ReadToEndAsync();
    }
  }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using CurrentIp.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrentIp.Web.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class CurrentIpController : ControllerBase {
    private readonly ILogger<CurrentIpController> _logger;
    private readonly IRecordsRepository _recordsRepository;

    public CurrentIpController(ILogger<CurrentIpController> logger, IRecordsRepository recordsRepository) {
      _logger = logger;
      _recordsRepository = recordsRepository;
    }

    [HttpPost("report")]
    public async Task<IActionResult> AddReport(Report report, CancellationToken token) {
      _logger.LogInformation($"Received a report from {report.MachineName} with {report.CurrentIP}");
      var record = await _recordsRepository.CreateAsync(report, token).ConfigureAwait(false);
      return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{machineTag}/history")]
    public async Task<ActionResult<IEnumerable<IpRecord>>> GetHistory(string machineTag, CancellationToken token) {
      var history = await _recordsRepository.GetHistoryAsync(machineTag, token).ConfigureAwait(false);
      return new OkObjectResult(history);
    }

    [HttpGet("{machineTag}/latest")]
    public async Task<ActionResult<IpRecord>> GetLatest(string machineTag, CancellationToken token) {
      var latestRecord = await _recordsRepository.GetLatestAsync(machineTag,token).ConfigureAwait(false);
      if (latestRecord is null) {
        return new IpRecord {
          CurrentIP = new IPAddress(new byte[] {192, 168, 1, 1}).ToString(),
          LastSeen = DateTime.Now,
          MachineName = "dummy"
        };
      }

      return latestRecord;
    }

    [HttpGet("{machineTag}/rdp")]
    public async Task<IActionResult> GetRdpFile(string machineTag, CancellationToken token) {
      var latestRecord = await _recordsRepository.GetLatestAsync(machineTag, token).ConfigureAwait(false);
      var sb = new StringBuilder();
      sb.AppendLine($"full address:s:{latestRecord.CurrentIP}:3389");
      sb.AppendLine("prompt for credentials:i:1");
      sb.AppendLine("administrative session:i:1");

      var stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString()));
      return File(stream, "application/octet-stream",$"{latestRecord.MachineName}.rdp");
    }
  }
}
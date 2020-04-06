using System;
using System.Collections.Generic;
using System.Net;
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
      var record = await _recordsRepository.CreateAsync(report, token).ConfigureAwait(false);
      return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{machineName}/history")]
    public async Task<ActionResult<IEnumerable<IpRecord>>> GetHistory(string machineName, CancellationToken token) {
      var history = await _recordsRepository.GetHistoryAsync(machineName, token).ConfigureAwait(false);
      return new OkObjectResult(history);
    }

    [HttpGet("{blah}/bar")]
    public async Task<OkResult> Bar(string blah) => Ok();

    [HttpGet("{machineName}/latest")]
    public async Task<ActionResult<IpRecord>> GetLatest(string machineName, CancellationToken token) {
      var latestRecord = await _recordsRepository.GetLatestAsync(machineName,token).ConfigureAwait(false);
      if (latestRecord is null) {
        return new IpRecord {
          CurrentIP = new IPAddress(new byte[] {192, 168, 1, 1}).ToString(),
          LastSeen = DateTime.Now,
          MachineName = "dummy"
        };
      }

      return latestRecord;
    }
  }
}
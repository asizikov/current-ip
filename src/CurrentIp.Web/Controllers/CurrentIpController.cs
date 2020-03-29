using System;

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using CurrentIp.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrentIp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentIpController : ControllerBase
    {
        private readonly ILogger<CurrentIpController> _logger;
        private readonly IRecordsRepository _recordsRepository;

        public CurrentIpController(ILogger<CurrentIpController> logger, IRecordsRepository recordsRepository)
        {
            _logger = logger;
            _recordsRepository = recordsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Report ipRecord, CancellationToken token)
        {
            var record = await _recordsRepository.CreateAsync(ipRecord, token).ConfigureAwait(false);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("history")]
        public async Task<ActionResult> GetHistory()
        {
            return new AcceptedResult();
        }

        [HttpGet("latest")]
        public async Task<IpRecord> GetLatest()
        {
            return new IpRecord
            {
                CurrentIP = new IPAddress(new byte[] {192, 168, 1, 1}).ToString(),
                LastSeen = DateTime.Now,
                MachineName = "name"
            };
        }
    }
}
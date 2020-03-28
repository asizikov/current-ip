using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using CurrentIp.Storage;
using CurrentIp.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using Xunit;

namespace CurrentIp.Web.UnitTests
{
    public class CurrentIpControllerTests
    {
        private readonly Mock<IRecordsRepository> _mockRepository;
        private readonly CurrentIpController _controller;
        private readonly CancellationToken _token;

        public CurrentIpControllerTests()
        {
            _mockRepository = new Mock<IRecordsRepository>();
            _controller = new CurrentIpController(new NullLogger<CurrentIpController>(), _mockRepository.Object);
            _token = CancellationToken.None;
        }

        [Fact]
        public async Task Test()
        {
            var report = new Report
            {
                CurrentIP = IPAddress.Loopback.ToString(),
                MachineName = "domain/name"
            };
            
            var actionResult = await _controller.Post(report, _token).ConfigureAwait(false);
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldNotBeNull(),
                () => actionResult.ShouldBeAssignableTo<StatusCodeResult>(),
                () => (actionResult as StatusCodeResult).StatusCode.ShouldBe(StatusCodes.Status201Created));
            _mockRepository.Verify(repo => repo.CreateAsync(report,_token), Times.Once);
        }

        [Fact]
        public async Task GetLatest_Returns_Record_If_Present()
        {
            var ipRecord = await _controller.GetLatest().ConfigureAwait(false);
            ipRecord.ShouldSatisfyAllConditions(
                () => ipRecord.ShouldNotBeNull()
            );
        }
    }
}
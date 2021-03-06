using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;
using CurrentIp.Storage;
using CurrentIp.Web.Controllers;
using CurrentIp.Web.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using Xunit;

namespace CurrentIp.Web.UnitTests {
  public class CurrentIpControllerTests {
    private readonly Mock<IRecordsRepository> _mockRepository;
    private readonly Mock<IRdpFileProvider> _mockRdpFileBuilder;

    private readonly CurrentIpController _controller;
    private readonly CancellationToken _token;

    public CurrentIpControllerTests() {
      _mockRepository = new Mock<IRecordsRepository>();
      _mockRdpFileBuilder = new Mock<IRdpFileProvider>();
      _controller = new CurrentIpController(new NullLogger<CurrentIpController>(), _mockRepository.Object, _mockRdpFileBuilder.Object);
      _token = CancellationToken.None;
    }

    [Fact]
    public async Task CreateReport() {
      var report = new Report {
        CurrentIP = IPAddress.Loopback.ToString(),
        MachineName = "domain/name"
      };

      var actionResult = await _controller.AddReport(report, _token).ConfigureAwait(false);
      actionResult.ShouldSatisfyAllConditions(
        () => actionResult.ShouldNotBeNull(),
        () => actionResult.ShouldBeAssignableTo<StatusCodeResult>(),
        () => (actionResult as StatusCodeResult).StatusCode.ShouldBe(StatusCodes.Status201Created));
      _mockRepository.Verify(repo => repo.CreateAsync(report, _token), Times.Once);
    }

    [Fact]
    public async Task GetLatest_Returns_Record_If_Present() {
      const string machineTag = "domain-name";
      var expectedRecord = BuildExpectedRecord();

      _mockRepository.Setup(repo => repo.GetLatestAsync(machineTag, _token))
        .ReturnsAsync(expectedRecord);
      var result = await _controller.GetLatest(machineTag, _token).ConfigureAwait(false);
      var ipRecord = result.Value;
      ipRecord.ShouldSatisfyAllConditions(
        () => ipRecord.ShouldNotBeNull(),
        () => ipRecord.CurrentIP.ShouldBe(expectedRecord.CurrentIP),
        () => ipRecord.MachineName.ShouldBe(expectedRecord.MachineName),
        () => ipRecord.LastSeen.ShouldBe(expectedRecord.LastSeen)
      );
      _mockRepository.Verify(repo => repo.GetLatestAsync(machineTag, _token), Times.Once);
    }

    [Fact]
    public async Task Get_RDP_Returns_Valid_File() {
      const string machineTag = "domain-name";
      var expectedRecord = BuildExpectedRecord();
      _mockRepository.Setup(repo => repo.GetLatestAsync(machineTag, _token))
        .ReturnsAsync(expectedRecord);
      _mockRdpFileBuilder.Setup(builder => builder.CreateFileStreamAsync(expectedRecord, _token))
        .ReturnsAsync(Stream.Null);
      var actionResult = await _controller.GetRdpFile(machineTag, _token).ConfigureAwait(false);
      actionResult.ShouldBeAssignableTo<FileStreamResult>();
      var fileResult = actionResult as FileStreamResult;

      fileResult.ShouldSatisfyAllConditions(
        () => fileResult.ContentType.ShouldBe("application/octet-stream"),
        () => fileResult.FileDownloadName.ShouldBe($"{machineTag}.rdp")
      );
    }

    private static IpRecord BuildExpectedRecord() =>
      new IpRecord {
        CurrentIP = "127.0.0.1",
        LastSeen = DateTime.Now.AddDays(-1),
        MachineName = "domain/name"
      };
  }
}
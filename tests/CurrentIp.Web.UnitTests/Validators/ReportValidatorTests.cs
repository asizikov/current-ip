using System.Net;
using CurrentIp.DataModel;
using CurrentIp.Web.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace CurrentIp.Web.UnitTests.Validators {
  public class ReportValidatorTests {
    private readonly ReportValidator _validator;

    public ReportValidatorTests() {
      _validator = new ReportValidator();
    }

    [Fact]
    public void When_IpAddress_Null_ShouldHaveError() {
      _validator.ShouldHaveValidationErrorFor(m => m.CurrentIP, null as string);
    }

    [Theory]
    [InlineData("    ")]
    [InlineData("ab.vxy.123.0")]
    [InlineData("12312.123123.123123")]
    [InlineData("999.1.1.0")]
    public void When_IpAddress_Invalid_ShouldHaveError(string value) {
      _validator.ShouldHaveValidationErrorFor(m => m.CurrentIP, value);
    }

    [Fact]
    public void When_MachineName_Null_ShouldHaveError() {
      _validator.ShouldHaveValidationErrorFor(m => m.MachineName, null as string);
    }

    [Fact]
    public void When_MachineName_Too_Long_ShouldHaveError() {
      _validator.ShouldHaveValidationErrorFor(m => m.MachineName, new string(new char[300]));
    }

    [Theory]
    [InlineData("    ")]
    [InlineData("adsf/adsf")]
    [InlineData("asdfasdf-")]
    [InlineData("abc\\dc")]
    [InlineData("adf/asdf")]
    [InlineData("asdf#adsf")]
    [InlineData("-asdfasdf")]
    public void When_MachineTag_Invalid_ShouldHaveError(string value) {
      _validator.ShouldHaveValidationErrorFor(m => m.MachineTag, value);
    }

    [Fact]
    public void Valid_Model_Now_Errors() {
      var testValidationResult = _validator.TestValidate(new Report {
        CurrentIP = new IPAddress(new byte[] {192, 168, 1, 1}).ToString(),
        MachineName = "Valid/Name",
        MachineTag = "valid-tag"
      });
      testValidationResult.IsValid.ShouldBeTrue();
    }
  }
}
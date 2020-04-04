using CurrentIp.DataModel;
using FluentValidation;
using FluentValidation.Validators;

namespace CurrentIp.Web.Validators {
  public static class FluentValidatorRuleBuilderExtensions {
    public static IRuleBuilderOptions<Report, string> ValidIpAddress(this IRuleBuilder<Report, string> ruleBuilder)
      => ruleBuilder.SetValidator(new IpAddressValidator(default(string)));
  }
}
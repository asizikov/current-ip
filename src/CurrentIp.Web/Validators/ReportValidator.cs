using System.Runtime.Serialization;
using CurrentIp.DataModel;
using FluentValidation;

namespace CurrentIp.Web.Validators
{
    public class ReportValidator : AbstractValidator<Report>
    {
        public ReportValidator()
        {
            RuleFor(x => x.CurrentIP).NotNull();
            RuleFor(x => x.CurrentIP);
            RuleFor(x => x.MachineName).NotEmpty().MaximumLength(256);
        }
    }

    public static class FluentValidatorRuleBuilderExtensions
    {
        public static IRuleBuilderOptions<Report, string> ValidIpAddress(this IRuleBuilder<Report, string> ruleBuilder)
        {
            return null;
        }
    }
}
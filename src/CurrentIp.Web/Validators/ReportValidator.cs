using System;
using System.Runtime.Serialization;
using CurrentIp.DataModel;
using FluentValidation;
using FluentValidation.Resources;

namespace CurrentIp.Web.Validators {
  public class ReportValidator : AbstractValidator<Report> {
    public ReportValidator() {
      RuleFor(x => x.CurrentIP).NotNull();
      RuleFor(x => x.CurrentIP).ValidIpAddress();
      RuleFor(x => x.MachineName).NotEmpty().MaximumLength(256);
      RuleFor(x => x.MachineTag).NotEmpty().MaximumLength(256);
      RuleFor(x => x.MachineTag)
        .Matches(@"^([a-zA-Z]+|\d+)+([a-zA-Z]*|\d*|[-@]*)*([a-zA-Z]+|\d+)+$");
    }
  }
}
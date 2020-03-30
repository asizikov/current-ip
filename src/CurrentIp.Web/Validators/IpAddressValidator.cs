using System.Net;
using FluentValidation.Validators;

namespace CurrentIp.Web.Validators
{
    public class IpAddressValidator : PropertyValidator, INotEmptyValidator, IPropertyValidator
    {
        public IpAddressValidator(object defaultValueForType)
            : base("Should be a valid IP address value")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var propertyValue = context.PropertyValue;
            return !(propertyValue is null) && IPAddress.TryParse(propertyValue as string, out _ );
        }
    }
}
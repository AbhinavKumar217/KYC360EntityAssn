using FluentValidation;
using FluentValidation.Validators;
using KYC360Assn.Models.Request;

namespace KYC360Assn.Utils.Validations
{
    internal class NewAddressesRequestValidator : AbstractValidator<NewAddressesRequest>
    {
        public NewAddressesRequestValidator()
        {
            RuleFor(address => address.AddressLine).NotEmpty().MaximumLength(100);
            RuleFor(address => address.City).NotEmpty().MaximumLength(50);
            RuleFor(address => address.Country).NotEmpty().MaximumLength(50);
        }
    }
}
using FluentValidation;
using KYC360Assn.Models.Request;

namespace KYC360Assn.Utils.Validations
{
    public class NewEntityRequestBodyValidator : AbstractValidator<NewEntityRequestBody>
    {
        public NewEntityRequestBodyValidator() {
            RuleFor(request => request.Names).NotEmpty();
            RuleForEach(request => request.Names).SetValidator(new NewNamesRequestValidator());
            RuleForEach(request => request.Addresses).SetValidator(new NewAddressesRequestValidator());
            RuleForEach(request => request.Dates).SetValidator(new NewDateRequestValidator());
            RuleFor(request => request.Gender).NotEmpty().MaximumLength(10);
        }
    }
}

using FluentValidation;
using FluentValidation.Validators;
using KYC360Assn.Models.Request;

namespace KYC360Assn.Utils.Validations
{
    internal class NewDateRequestValidator : AbstractValidator<NewDateRequest>
    {
        public NewDateRequestValidator()
        {
            RuleFor(date => date.DateType).NotEmpty().MaximumLength(50);
            RuleFor(date => date.DateValue).NotEmpty();
        }
    }
}
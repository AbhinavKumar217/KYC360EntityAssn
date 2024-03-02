using FluentValidation;
using FluentValidation.Validators;
using KYC360Assn.Models.Request;

namespace KYC360Assn.Utils.Validations
{
    internal class NewNamesRequestValidator : AbstractValidator<NewNamesRequest>
    {
        public NewNamesRequestValidator()
        {
            RuleFor(name => name.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(name => name.MiddleName).MaximumLength(50);
            RuleFor(name => name.Surname).NotEmpty().MaximumLength(50);
        }
    }
}
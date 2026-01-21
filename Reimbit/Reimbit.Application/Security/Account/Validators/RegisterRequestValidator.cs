using FluentValidation;
using Reimbit.Contracts.Account;

namespace Reimbit.Application.Security.Account.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.LastName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.MobileNo).Length(10).WithMessage("{PropertyName} must be length of 10").NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.OrganizationName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.Email).EmailAddress().WithMessage("{PropertyName} is not valid email").NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.Password).MinimumLength(6).WithMessage("{PropertyName} must be at least {MinimumLength}").NotEmpty().WithMessage("{PropertyName} is required");
    }
}

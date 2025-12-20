using FluentValidation;
using Reimbit.Contracts.Security.Account;

namespace Reimbit.Application.Security.Account.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.MobileNo).Length(10).NotEmpty();
        RuleFor(c => c.OrganizationName).NotEmpty();
        RuleFor(c => c.Email).EmailAddress().NotEmpty();
        RuleFor(c => c.Password).MinimumLength(6).NotEmpty();
    }
}

using FluentValidation;
using Reimbit.Contracts.Security.Account;

namespace Reimbit.Application.Security.Account.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(c => c.Email).EmailAddress().NotEmpty();
        RuleFor(c => c.Password).MinimumLength(6).NotEmpty();
    }
}

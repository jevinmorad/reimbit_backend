using FluentValidation;
using Reimbit.Contracts.Account;

namespace Reimbit.Application.Security.Account.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(c => c.Email).EmailAddress().WithMessage("{PropertyName} is not valid email").NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.Password).MinimumLength(6).WithMessage("{PropertyName} must be atleast 6 length").NotEmpty().WithMessage("{PropertyName} is required");
    }
}

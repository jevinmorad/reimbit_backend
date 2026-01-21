using FluentValidation;
using Reimbit.Contracts.Role;

namespace Reimbit.Application.EmployeeManagement.Role.Validators;

public class InsertRequestValidator : AbstractValidator<InsertRoleRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .MaximumLength(50)
            .WithMessage("{PropertyName} must at most {MaxLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .WithMessage("{PropertyName} must at most {MaxLength} characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
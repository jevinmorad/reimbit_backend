using FluentValidation;
using Reimbit.Contracts.Role;

namespace Reimbit.Application.EmployeeManagement.Role.Validators;

public sealed class InsertRoleRequestValidator : AbstractValidator<InsertRoleRequest>
{
    public InsertRoleRequestValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} must be at most 50 characters");

        RuleFor(x => x.Description)
            .MaximumLength(100).When(x => x.Description != null)
            .WithMessage("{PropertyName} must be at most 100 characters");

        RuleForEach(x => x.PermissionValues)
            .GreaterThan(0).WithMessage("PermissionValues must contain valid values.");
    }
}
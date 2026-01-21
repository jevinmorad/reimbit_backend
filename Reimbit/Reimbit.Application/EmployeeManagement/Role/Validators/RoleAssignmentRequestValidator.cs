using FluentValidation;
using Reimbit.Contracts.Role;

namespace Reimbit.Application.EmployeeManagement.Role.Validators;

public sealed class RoleAssignmentRequestValidator : AbstractValidator<UserRoleAssignmentRequest>
{
    public RoleAssignmentRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("{PropertyName} is required");
    }
}
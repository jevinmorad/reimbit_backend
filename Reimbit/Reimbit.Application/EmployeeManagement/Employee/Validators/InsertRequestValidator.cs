using FluentValidation;
using Reimbit.Contracts.Employee;

namespace Reimbit.Application.EmployeeManagement.Employee.Validators;

public class InsertRequestValidator : AbstractValidator<EmployeeInsertRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.LastName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.Email).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.MobileNo).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.RoleId).NotEmpty().WithMessage("{PropertyName} is required");

        When(x => x.ManagerId != null, () =>
        {
            RuleFor(x => x.ManagerType).NotNull().WithMessage("{PropertyName} is required");
            RuleFor(x => x.ManagerValidTo).Cascade(CascadeMode.Stop).NotNull().WithMessage("{PropertyName} is required")
                .GreaterThan(x => x.ManagerValidFrom)
                .When(x => x.ManagerValidFrom != null && x.ManagerValidTo != null)
                .WithMessage("ManagerValidTo must be after ManagerValidFrom.");
        });
    }
}
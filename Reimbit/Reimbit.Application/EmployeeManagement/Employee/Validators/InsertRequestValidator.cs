using FluentValidation;
using Reimbit.Contracts.Employee;

namespace Reimbit.Application.EmployeeManagement.Employee.Validator;

public class InsertRequestValidator : AbstractValidator<InsertEmployeeRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.LastName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.Email).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.MobileNo).NotEmpty().WithMessage("{PropertyName} is required");
        //RuleFor(c => c.RoleId).NotEmpty().WithMessage("{PropertyName} is required");
    }
}
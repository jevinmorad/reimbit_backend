using FluentValidation;
using Reimbit.Contracts.Employee;

namespace Reimbit.Application.EmployeeManagement.Employee.Validator;

public class InsertRequestValidator : AbstractValidator<InsertEmployeeRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Email).NotEmpty();
        RuleFor(c => c.MobileNo).NotEmpty();
        //RuleFor(c => c.RoleId).NotEmpty();
    }
}
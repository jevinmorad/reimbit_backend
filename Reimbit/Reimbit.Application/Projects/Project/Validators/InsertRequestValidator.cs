using FluentValidation;
using Reimbit.Contracts.Project;

namespace Reimbit.Application.Projects.Project.Validators;

public class InsertRequestValidator : AbstractValidator<InsertRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(c => c.ProjectName).NotEmpty();
        RuleFor(c => c.ManagerId).NotEmpty();
    }
}

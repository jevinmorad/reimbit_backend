using FluentValidation;
using Reimbit.Contracts.Project;

namespace Reimbit.Application.Projects.Project.Validators;

public class InsertRequestValidator : AbstractValidator<InsertProjectRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(c => c.ProjectName).NotEmpty();
        RuleFor(c => c.ManagerId).NotEmpty();
    }
}

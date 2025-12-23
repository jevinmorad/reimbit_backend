using FluentValidation;
using Reimbit.Contracts.Project;

namespace Reimbit.Application.Projects.Project.Validators;

public class InsertRequestValidator : AbstractValidator<InsertProjectRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(c => c.ProjectName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.ManagerId).NotEmpty().WithMessage("{PropertyName} is required");
    }
}

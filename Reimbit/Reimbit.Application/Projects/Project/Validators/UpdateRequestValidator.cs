using FluentValidation;
using Reimbit.Contracts.Project;

namespace Reimbit.Application.Projects.Project.Validators;

public class UpdateRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(c => c.ProjectId).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.ProjectName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(c => c.ManagerId).NotEmpty().WithMessage("{PropertyName} is required");
        //RuleFor(c => c.IsActive).NotEmpty().WithMessage("{PropertyName} is required");
    }
}

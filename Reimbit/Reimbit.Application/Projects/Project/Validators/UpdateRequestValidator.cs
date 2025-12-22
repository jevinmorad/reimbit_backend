using FluentValidation;
using Reimbit.Contracts.Project;

namespace Reimbit.Application.Projects.Project.Validators;

public class UpdateRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(c => c.ProjectId).NotEmpty();
        RuleFor(c => c.ProjectName).NotEmpty();
        RuleFor(c => c.ManagerId).NotEmpty();
        //RuleFor(c => c.IsActive).NotEmpty();
    }
}

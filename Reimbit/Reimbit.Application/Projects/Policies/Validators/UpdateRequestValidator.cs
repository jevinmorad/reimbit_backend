using FluentValidation;
using Reimbit.Contracts.Policies;

namespace Reimbit.Application.Projects.Policies.Validators;

public class UpdateRequestValidator : AbstractValidator<UpdatePolicyRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.PolicyId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.MaxAmount).GreaterThan(0).When(x => x.MaxAmount.HasValue);
    }
}

using FluentValidation;
using Reimbit.Contracts.Policies;

namespace Reimbit.Application.Projects.Policies.Validators;

public class InsertRequestValidator : AbstractValidator<InsertPolicyRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.MaxAmount).GreaterThan(0).When(x => x.MaxAmount.HasValue);
    }
}

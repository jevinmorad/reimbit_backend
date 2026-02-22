using FluentValidation;
using Reimbit.Contracts.Policies;

namespace Reimbit.Application.Projects.Policies.Validators;

public class InsertRequestValidator : AbstractValidator<PolicyInsertRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.MaxAmount).GreaterThan(0).WithMessage("{PropertyName} must be greater than 0").When(x => x.MaxAmount.HasValue);
    }
}

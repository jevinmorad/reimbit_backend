using FluentValidation;
using Reimbit.Contracts.Approvals.Delegation;

namespace Reimbit.Application.Approvals.Delegation.Validators;

public sealed class RevokeDelegationRequestValidator : AbstractValidator<RevokeDelegationRequest>
{
    public RevokeDelegationRequestValidator()
    {
        RuleFor(x => x.DelegateId).NotEmpty().WithMessage("{PropertyName} is required");
    }
}
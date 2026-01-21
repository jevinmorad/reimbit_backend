using FluentValidation;
using Reimbit.Contracts.Approvals.Delegation;

namespace Reimbit.Application.Approvals.Delegation.Validators;

public sealed class CreateDelegationRequestValidator : AbstractValidator<CreateDelegationRequest>
{
    private const int MinDays = 1;
    private const int MaxDays = 60;

    public CreateDelegationRequestValidator()
    {
        RuleFor(x => x.DelegateUserId).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.ValidFrom).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.ValidTo).NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(x => x.ValidTo)
            .GreaterThan(x => x.ValidFrom)
            .WithMessage("ValidTo must be after ValidFrom.");

        RuleFor(x => x)
            .Must(x => (x.ValidTo.Date - x.ValidFrom.Date).Days + 1 >= MinDays)
            .WithMessage($"Delegation must be at least {MinDays} day(s).");

        RuleFor(x => x)
            .Must(x => (x.ValidTo.Date - x.ValidFrom.Date).Days + 1 <= MaxDays)
            .WithMessage($"Delegation cannot exceed {MaxDays} day(s).");
    }
}
using FluentValidation;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Application.Expenses.Expense.Validators;

public class InsertRequestValidator : AbstractValidator<InsertExpenseRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.Title).NotEmpty().WithMessage("{PropertyName} is required").MaximumLength(250).WithMessage("{PropertyName} is required");
        RuleFor(x => x.Amount).NotEmpty().WithMessage("{PropertyName} is required").GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
    }
}

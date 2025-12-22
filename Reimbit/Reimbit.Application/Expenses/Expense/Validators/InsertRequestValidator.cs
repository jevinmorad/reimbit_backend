using FluentValidation;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Application.Expenses.Expense.Validators;

public class InsertRequestValidator : AbstractValidator<InsertExpenseRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}

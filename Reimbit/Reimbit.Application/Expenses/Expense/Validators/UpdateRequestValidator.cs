using FluentValidation;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Application.Expenses.Expense.Validators;

public class UpdateRequestValidator : AbstractValidator<UpdateExpenseRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.ExpenseId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}

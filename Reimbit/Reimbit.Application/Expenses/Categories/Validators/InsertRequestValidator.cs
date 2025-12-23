using FluentValidation;
using Reimbit.Contracts.ExpenseCategories;

namespace Reimbit.Application.Expenses.Validators;

public class InsertRequestValidator : AbstractValidator<InsertExpenseCategoriesRequest>
{
    public InsertRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("{PropertyName} is required");
    }
}

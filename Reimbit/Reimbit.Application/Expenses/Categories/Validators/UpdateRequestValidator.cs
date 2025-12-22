using FluentValidation;
using Reimbit.Contracts.ExpenseCategories;

namespace Reimbit.Application.Expenses.Categories.Validators;

public class UpdateRequestValidator : AbstractValidator<UpdateExpenseCategoriesRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.CategoryName).NotEmpty();
    }
}

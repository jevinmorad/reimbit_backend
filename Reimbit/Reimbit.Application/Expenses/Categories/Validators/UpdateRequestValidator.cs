using FluentValidation;
using Reimbit.Contracts.ExpenseCategories;

namespace Reimbit.Application.Expenses.Categories.Validators;

public class UpdateRequestValidator : AbstractValidator<UpdateExpenseCategoriesRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("{PropertyName} is required");
    }
}

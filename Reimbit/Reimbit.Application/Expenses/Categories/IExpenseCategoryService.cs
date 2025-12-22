using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.ExpenseCategories;

namespace Reimbit.Application.Expenses.Categories;

public interface IExpenseCategoryService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseCategoriesRequest request);
    Task<ErrorOr<PagedResult<ListExpenseCategoriesResponse>>> List();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseCategoriesRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt categoryId);
    Task<ErrorOr<GetExpenseCategoriesResponse>> Get(EncryptedInt categoryId);
}
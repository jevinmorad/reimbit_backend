using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.ExpenseCategories;

namespace Reimbit.Domain.Repositories;

public interface IExpenseCategoryRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseCategoriesRequest request);
    Task<ErrorOr<PagedResult<ListExpenseCategoriesResponse>>> List(int userId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseCategoriesRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt categoryId);
    Task<ErrorOr<GetExpenseCategoriesResponse>> Get(EncryptedInt categoryId);
    Task<ErrorOr<List<OptionsResponse<EncryptedInt>>>> SelectComboBox(int organizationId);
}
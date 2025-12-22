using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Expenses.Categories;

namespace Reimbit.Domain.Repositories;

public interface IExpenseCategoryRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request);
    Task<ErrorOr<PagedResult<ListResponse>>> List(int userId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt categoryId);
    Task<ErrorOr<GetResponse>> Get(EncryptedInt categoryId);
}
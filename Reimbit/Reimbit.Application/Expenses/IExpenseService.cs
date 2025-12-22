using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Application.Expenses;

public interface IExpenseService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request);
    Task<ErrorOr<PagedResult<ListResponse>>> List();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId);
    Task<ErrorOr<GetResponse>> Get(EncryptedInt expenseId);
}
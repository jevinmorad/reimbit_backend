using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Domain.Repositories;

public interface IExpenseRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseRequest request);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> List(ListExpenseRequest request);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByOrganization(int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId);
    Task<ErrorOr<GetExpenseResponse>> Get(EncryptedInt expenseId);
    Task<ErrorOr<ViewExpenseResponse>> View(EncryptedInt expenseId);
}
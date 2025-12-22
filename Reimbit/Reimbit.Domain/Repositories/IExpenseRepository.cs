using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Domain.Repositories;

public interface IExpenseRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseRequest request);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByUserId(EncryptedInt userId);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByProject(EncryptedInt projectId);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByOrganization(int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId);
    Task<ErrorOr<GetExpenseResponse>> Get(EncryptedInt expenseId);
    Task<ErrorOr<ViewExpenseResponse>> View(EncryptedInt expenseId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Accept(EncryptedInt request, int modifiedByUserId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(RejectExpenseRequest request);
}
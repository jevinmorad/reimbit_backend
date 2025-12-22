using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Application.Expenses.Expense;

public interface IExpenseService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseRequest request);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListMyExpenses();
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByUserId(EncryptedInt userId);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByProject(EncryptedInt projectId);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByOrganization();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId);
    Task<ErrorOr<GetExpenseResponse>> Get(EncryptedInt expenseId);
    Task<ErrorOr<ViewExpenseResponse>> View(EncryptedInt expenseId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Accept(AcceptExpenseRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(RejectExpenseRequest request);
}
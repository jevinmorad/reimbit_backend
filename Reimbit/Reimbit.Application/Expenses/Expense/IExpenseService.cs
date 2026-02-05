using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Application.Expenses.Expense;

public interface IExpenseService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseRequest request);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListExpenses(ListExpenseRequest request);
    Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByOrganization();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId);
    Task<ErrorOr<GetExpenseResponse>> Get(EncryptedInt expenseId);
    Task<ErrorOr<ViewExpenseResponse>> View(EncryptedInt expenseId);
}
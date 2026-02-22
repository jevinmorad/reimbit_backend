using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Domain.Repositories;

public interface IExpenseRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(ExpenseInsertRequest request);
    Task<ErrorOr<PagedResult<ExpensesSelectPageResponse>>> SelectPage(ExpenseSelectPageRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(ExpenseUpdateRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId);
    Task<ErrorOr<ExpenseSelectPkResponse>> Get(EncryptedInt expenseId);
    Task<ErrorOr<ExpenseSelectViewResponse>> View(EncryptedInt expenseId);
}
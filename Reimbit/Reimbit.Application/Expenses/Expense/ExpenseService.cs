using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.Expenses;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Expenses.Expense;

public class ExpenseService(
    ICurrentUserProvider currentUserProvider,
    IExpenseRepository repository
) : IExpenseService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(ExpenseInsertRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.UserId = currentUser.UserId;
        request.CreatedByUserId = currentUser.UserId;
        request.ModifiedByUserId = currentUser.UserId;
        request.Created = DateTime.UtcNow;
        request.Modified = DateTime.UtcNow;

        return await repository.Insert(request);
    }

    public async Task<ErrorOr<PagedResult<ExpensesSelectPageResponse>>> SelectPage(ExpenseSelectPageRequest request)
    {
        request.UserID = currentUser.UserId;
        return await repository.SelectPage(request);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(ExpenseUpdateRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.ModifiedByUserId = currentUser.UserId;

        return await repository.Update(request);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId)
        => await repository.Delete(expenseId);

    public async Task<ErrorOr<ExpenseSelectPkResponse>> Get(EncryptedInt expenseId)
        => await repository.Get(expenseId);

    public async Task<ErrorOr<ExpenseSelectViewResponse>> View(EncryptedInt expenseId)
        => await repository.View(expenseId);
}
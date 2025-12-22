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

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.UserId = currentUser.UserId;
        request.CreatedByUserId = currentUser.UserId;
        request.ModifiedByUserId = currentUser.UserId;
        request.Created = DateTime.UtcNow;
        request.Modified = DateTime.UtcNow;
        request.ExpenseStatus = "submitted";

        return await repository.Insert(request);
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListMyExpenses()
    {
        return await repository.ListByUserId(currentUser.UserId);
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByUserId(EncryptedInt userId)
    {
        return await repository.ListByUserId(userId);
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByProject(EncryptedInt projectId)
    {
        return await repository.ListByProject(projectId);
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByOrganization()
    {
        return await repository.ListByOrganization(currentUser.OrganizationId);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.ModifiedByUserId = currentUser.UserId;
        request.Modified = DateTime.UtcNow;

        return await repository.Update(request);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId)
    {
        return await repository.Delete(expenseId);
    }

    public async Task<ErrorOr<GetExpenseResponse>> Get(EncryptedInt expenseId)
    {
        return await repository.Get(expenseId);
    }

    public async Task<ErrorOr<ViewExpenseResponse>> View(EncryptedInt expenseId)
    {
        return await repository.View(expenseId);
    }
}
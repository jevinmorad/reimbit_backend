using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.ExpenseReports;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.ExpenseReports;

public sealed class ExpenseReportService(
    ICurrentUserProvider currentUserProvider,
    IExpenseReportRepository repository
) : IExpenseReportService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Create(CreateExpenseReportRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.CreatedByUserId = currentUser.UserId;
        request.ModifiedByUserId = currentUser.UserId;
        request.Created = DateTime.UtcNow;
        request.Modified = DateTime.UtcNow;

        return repository.Create(request);
    }

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> AddExpense(AddExpenseToReportRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.UserId = currentUser.UserId;
        return repository.AddExpense(request);
    }

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> RemoveExpense(RemoveExpenseFromReportRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.UserId = currentUser.UserId;
        return repository.RemoveExpense(request);
    }

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Submit(SubmitExpenseReportRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.UserId = currentUser.UserId;
        return repository.Submit(request);
    }

    public Task<ErrorOr<GetExpenseReportResponse>> Get(EncryptedInt reportId)
        => repository.Get(reportId, currentUser.OrganizationId);
}
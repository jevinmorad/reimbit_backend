using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.Finance;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Finance;

public sealed class FinanceService(
    ICurrentUserProvider currentUserProvider,
    IFinanceRepository repository
) : IFinanceService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public Task<ErrorOr<PagedResult<PayableReportResponse>>> PayableReports()
        => repository.ListPayableReports(currentUser.OrganizationId);

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> ProcessPayout(ProcessPayoutRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.ProcessedByUserId = currentUser.UserId;
        return repository.ProcessPayout(request);
    }

    public Task<ErrorOr<PagedResult<PayoutHistoryResponse>>> PayoutHistory(EncryptedInt reportId)
        => repository.PayoutHistory(currentUser.OrganizationId, reportId);

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> ReversePayout(EncryptedInt reportId, string reason)
        => repository.ReversePayout(reportId, currentUser.OrganizationId, currentUser.UserId, reason);
}
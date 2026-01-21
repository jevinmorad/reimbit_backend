using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.Approvals;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Approvals;

public sealed class ApprovalService(
    ICurrentUserProvider currentUserProvider,
    IApprovalRepository repository
) : IApprovalService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public Task<ErrorOr<PagedResult<ApprovalInboxItemResponse>>> Inbox()
        => repository.Inbox(currentUser.UserId, currentUser.OrganizationId);

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Approve(ApproveApprovalRequest request)
        => repository.Approve(request.ApprovalInstanceId, currentUser.UserId, currentUser.OrganizationId);

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(RejectApprovalRequest request)
        => repository.Reject(request.ApprovalInstanceId, request.Reason, currentUser.UserId, currentUser.OrganizationId);
}
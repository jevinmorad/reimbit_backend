using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Approvals;

namespace Reimbit.Domain.Repositories;

public interface IApprovalRepository
{
    Task<ErrorOr<PagedResult<ApprovalInboxItemResponse>>> Inbox(int approverUserId, int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Approve(EncryptedInt approvalInstanceId, int approverUserId, int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(EncryptedInt approvalInstanceId, string reason, int approverUserId, int organizationId);
}
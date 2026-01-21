using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Approvals;

namespace Reimbit.Application.Approvals;

public interface IApprovalService
{
    Task<ErrorOr<PagedResult<ApprovalInboxItemResponse>>> Inbox();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Approve(ApproveApprovalRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(RejectApprovalRequest request);
}
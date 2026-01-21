using AegisInt.Core;

namespace Reimbit.Contracts.Approvals;

public sealed class ApproveApprovalRequest
{
    public required EncryptedInt ApprovalInstanceId { get; set; }
}
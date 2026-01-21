using AegisInt.Core;

namespace Reimbit.Contracts.Approvals;

public sealed class RejectApprovalRequest
{
    public required EncryptedInt ApprovalInstanceId { get; set; }
    public required string Reason { get; set; }
}
namespace Reimbit.Contracts.Approvals;

public enum ApprovalInstanceStatus : byte
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Escalated = 4
}
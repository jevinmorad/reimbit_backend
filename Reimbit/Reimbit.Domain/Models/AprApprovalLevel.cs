namespace Reimbit.Domain.Models;

public partial class AprApprovalLevel
{
    public int ApprovalLevelId { get; set; }

    public int ApprovalRuleId { get; set; }

    public int LevelOrder { get; set; }

    public byte ApproverType { get; set; }

    public int? ApproverRoleId { get; set; }

    public int? SpecificUserId { get; set; }

    public int? EscalationAfterHours { get; set; }

    public byte? EscalateToApproverType { get; set; }

    public virtual AprApprovalRule ApprovalRule { get; set; } = null!;

    public virtual ICollection<AprApprovalInstance> AprApprovalInstances { get; set; } = new List<AprApprovalInstance>();
}

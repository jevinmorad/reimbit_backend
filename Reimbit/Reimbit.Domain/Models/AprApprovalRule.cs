namespace Reimbit.Domain.Models;

public partial class AprApprovalRule
{
    public int ApprovalRuleId { get; set; }

    public int OrganizationId { get; set; }

    public string RuleName { get; set; } = null!;

    public int Priority { get; set; }

    public decimal? MinAmount { get; set; }

    public decimal? MaxAmount { get; set; }

    public bool AppliesOnPolicyViolation { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AprApprovalLevel> AprApprovalLevels { get; set; } = new List<AprApprovalLevel>();

    public virtual OrgOrganization Organization { get; set; } = null!;
}

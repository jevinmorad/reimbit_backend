namespace Reimbit.Domain.Models;

public partial class AprApprovalInstance
{
    public int ApprovalInstanceId { get; set; }

    public int ReportId { get; set; }

    public int ApprovalLevelId { get; set; }

    public int ApproverUserId { get; set; }

    public byte Status { get; set; }

    public DateTime? ActionAt { get; set; }

    public virtual AprApprovalLevel ApprovalLevel { get; set; } = null!;

    public virtual SecUser ApproverUser { get; set; } = null!;

    public virtual ExpExpenseReport Report { get; set; } = null!;
}

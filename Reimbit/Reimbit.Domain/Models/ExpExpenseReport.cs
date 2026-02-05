namespace Reimbit.Domain.Models;

public partial class ExpExpenseReport
{
    public int ReportId { get; set; }

    public int OrganizationId { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public string Title { get; set; } = null!;

    public byte Status { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime? ViewedAt { get; set; }

    public int CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ICollection<AprApprovalInstance> AprApprovalInstances { get; set; } = new List<AprApprovalInstance>();

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual ICollection<ExpReportExpense> ExpReportExpenses { get; set; } = new List<ExpReportExpense>();

    public virtual ICollection<ExpReportRejection> ExpReportRejections { get; set; } = new List<ExpReportRejection>();

    public virtual SecUser ModifiedByUser { get; set; } = null!;

    public virtual OrgOrganization Organization { get; set; } = null!;

    public virtual ICollection<PayPayout> PayPayouts { get; set; } = new List<PayPayout>();
}

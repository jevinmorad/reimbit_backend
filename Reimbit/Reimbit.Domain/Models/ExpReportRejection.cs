namespace Reimbit.Domain.Models;

public partial class ExpReportRejection
{
    public int ReportRejectionId { get; set; }

    public int ReportId { get; set; }

    public int RejectedByUserId { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime RejectedAt { get; set; }

    public virtual SecUser RejectedByUser { get; set; } = null!;

    public virtual ExpExpenseReport Report { get; set; } = null!;
}

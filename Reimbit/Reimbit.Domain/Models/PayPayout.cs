namespace Reimbit.Domain.Models;

public partial class PayPayout
{
    public int PayoutId { get; set; }

    public int ReportId { get; set; }

    public decimal? PaidAmount { get; set; }

    public DateTime? PaidOn { get; set; }

    public string? ReferenceNo { get; set; }

    public virtual ExpExpenseReport Report { get; set; } = null!;
}

using AegisInt.Core;

namespace Reimbit.Contracts.Finance;

public sealed class PayableReportResponse
{
    public EncryptedInt ReportId { get; set; }
    public string Title { get; set; }
    public DateOnly PeriodStart { get; set; }
    public DateOnly PeriodEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Created { get; set; }
}
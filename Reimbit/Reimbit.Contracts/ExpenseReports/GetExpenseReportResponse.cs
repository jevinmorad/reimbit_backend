using AegisInt.Core;

namespace Reimbit.Contracts.ExpenseReports;

public class GetExpenseReportResponse
{
    public EncryptedInt ReportId { get; set; }
    public string Title { get; set; }
    public DateOnly PeriodStart { get; set; }
    public DateOnly PeriodEnd { get; set; }
    public byte Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Created { get; set; }
}
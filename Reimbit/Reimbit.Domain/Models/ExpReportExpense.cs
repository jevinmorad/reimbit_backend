namespace Reimbit.Domain.Models;

public partial class ExpReportExpense
{
    public int ReportExpenseId { get; set; }

    public int ReportId { get; set; }

    public int ExpenseId { get; set; }

    public virtual ExpExpense Expense { get; set; } = null!;

    public virtual ExpExpenseReport Report { get; set; } = null!;
}

namespace Reimbit.Domain.Models;

public partial class ExpExpenseRejection
{
    public int ExpenseRejectionId { get; set; }

    public int ExpenseId { get; set; }

    public int RejectedByUserId { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime RejectedAt { get; set; }

    public virtual ExpExpense Expense { get; set; } = null!;

    public virtual SecUser RejectedByUser { get; set; } = null!;
}

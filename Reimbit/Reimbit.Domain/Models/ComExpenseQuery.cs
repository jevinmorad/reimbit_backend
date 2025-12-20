namespace Reimbit.Domain.Models;

public partial class ComExpenseQuery
{
    public int QueryId { get; set; }

    public int ExpenseId { get; set; }

    public int SenderUserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? SentAt { get; set; }

    public bool? IsRead { get; set; }

    public virtual ExpExpense Expense { get; set; } = null!;

    public virtual SecUser SenderUser { get; set; } = null!;
}

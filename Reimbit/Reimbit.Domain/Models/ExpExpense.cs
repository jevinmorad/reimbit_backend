namespace Reimbit.Domain.Models;

public partial class ExpExpense
{
    public int ExpenseId { get; set; }

    public int OrganizationId { get; set; }

    public int EmployeeId { get; set; }

    public int? ProjectId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string ReceiptUrl { get; set; } = null!;

    public string? Description { get; set; }

    public byte Status { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual ExpCategory Category { get; set; } = null!;

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual SecUser Employee { get; set; } = null!;

    public virtual ICollection<ExpExpenseRejection> ExpExpenseRejections { get; set; } = new List<ExpExpenseRejection>();

    public virtual ICollection<ExpReportExpense> ExpReportExpenses { get; set; } = new List<ExpReportExpense>();

    public virtual OrgOrganization Organization { get; set; } = null!;

    public virtual ProjProject? Project { get; set; }
}

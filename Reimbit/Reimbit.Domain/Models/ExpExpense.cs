namespace Reimbit.Domain.Models;

public partial class ExpExpense
{
    public int ExpenseId { get; set; }

    public int OrganizationId { get; set; }

    public int UserId { get; set; }

    public int ProjectId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string AttachmentUrl { get; set; } = null!;

    public string? Description { get; set; }

    public string ExpenseStatus { get; set; } = null!;

    public string? RejectionReason { get; set; }

    public int CreatedByUserId { get; set; }

    public int ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual ExpCategory Category { get; set; } = null!;

    public virtual ICollection<ComExpenseQuery> ComExpenseQueries { get; set; } = new List<ComExpenseQuery>();

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual SecUser ModifiedByUser { get; set; } = null!;

    public virtual OrgOrganization Organization { get; set; } = null!;

    public virtual ProjProject Project { get; set; } = null!;

    public virtual SecUser User { get; set; } = null!;
}

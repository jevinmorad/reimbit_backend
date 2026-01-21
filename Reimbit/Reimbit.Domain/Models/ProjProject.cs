namespace Reimbit.Domain.Models;

public partial class ProjProject
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? ProjectLogoUrl { get; set; }

    public string? ProjectDetails { get; set; }

    public string? ProjectDescription { get; set; }

    public int OrganizationId { get; set; }

    public int ManagerId { get; set; }

    public bool IsArchived { get; set; }

    public int CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public bool IsActive { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual ICollection<ExpExpenseReport> ExpExpenseReports { get; set; } = new List<ExpExpenseReport>();

    public virtual ICollection<ExpExpense> ExpExpenses { get; set; } = new List<ExpExpense>();

    public virtual SecUser Manager { get; set; } = null!;

    public virtual SecUser ModifiedByUser { get; set; } = null!;

    public virtual OrgOrganization Organization { get; set; } = null!;
}

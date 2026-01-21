namespace Reimbit.Domain.Models;

public partial class OrgOrganization
{
    public int OrganizationId { get; set; }

    public string OrganizationName { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ICollection<AprApprovalRule> AprApprovalRules { get; set; } = new List<AprApprovalRule>();

    public virtual ICollection<ExpCategory> ExpCategories { get; set; } = new List<ExpCategory>();

    public virtual ICollection<ExpExpenseReport> ExpExpenseReports { get; set; } = new List<ExpExpenseReport>();

    public virtual ICollection<ExpExpense> ExpExpenses { get; set; } = new List<ExpExpense>();

    public virtual ICollection<ProjProject> ProjProjects { get; set; } = new List<ProjProject>();

    public virtual ICollection<SecRole> SecRoles { get; set; } = new List<SecRole>();

    public virtual ICollection<SecUser> SecUsers { get; set; } = new List<SecUser>();
}

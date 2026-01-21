namespace Reimbit.Domain.Models;

public partial class SecUser
{
    public int UserId { get; set; }

    public int OrganizationId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? MobileNo { get; set; }

    public string? ProfileImageUrl { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ICollection<AprApprovalInstance> AprApprovalInstances { get; set; } = new List<AprApprovalInstance>();

    public virtual SecUser? CreatedByUser { get; set; }

    public virtual ICollection<ExpCategory> ExpCategoryCreatedByUsers { get; set; } = new List<ExpCategory>();

    public virtual ICollection<ExpCategory> ExpCategoryModifiedByUsers { get; set; } = new List<ExpCategory>();

    public virtual ICollection<ExpExpense> ExpExpenseCreatedByUsers { get; set; } = new List<ExpExpense>();

    public virtual ICollection<ExpExpense> ExpExpenseEmployees { get; set; } = new List<ExpExpense>();

    public virtual ICollection<ExpExpenseRejection> ExpExpenseRejections { get; set; } = new List<ExpExpenseRejection>();

    public virtual ICollection<ExpExpenseReport> ExpExpenseReportCreatedByUsers { get; set; } = new List<ExpExpenseReport>();

    public virtual ICollection<ExpExpenseReport> ExpExpenseReportModifiedByUsers { get; set; } = new List<ExpExpenseReport>();

    public virtual ICollection<ExpPolicy> ExpPolicyCreatedByUsers { get; set; } = new List<ExpPolicy>();

    public virtual ICollection<ExpPolicy> ExpPolicyModifiedByUsers { get; set; } = new List<ExpPolicy>();

    public virtual ICollection<ExpReportRejection> ExpReportRejections { get; set; } = new List<ExpReportRejection>();

    public virtual ICollection<SecUser> InverseCreatedByUser { get; set; } = new List<SecUser>();

    public virtual ICollection<SecUser> InverseModifiedByUser { get; set; } = new List<SecUser>();

    public virtual SecUser? ModifiedByUser { get; set; }

    public virtual OrgOrganization? Organization { get; set; }

    public virtual ICollection<ProjProject> ProjProjectCreatedByUsers { get; set; } = new List<ProjProject>();

    public virtual ICollection<ProjProject> ProjProjectManagers { get; set; } = new List<ProjProject>();

    public virtual ICollection<ProjProject> ProjProjectModifiedByUsers { get; set; } = new List<ProjProject>();

    public virtual ICollection<SecDelegateApprover> SecDelegateApproverDelegateUsers { get; set; } = new List<SecDelegateApprover>();

    public virtual ICollection<SecDelegateApprover> SecDelegateApproverUsers { get; set; } = new List<SecDelegateApprover>();

    public virtual ICollection<SecRoleClaim> SecRoleClaims { get; set; } = new List<SecRoleClaim>();

    public virtual ICollection<SecRole> SecRoleCreatedByUsers { get; set; } = new List<SecRole>();

    public virtual ICollection<SecRole> SecRoleModifiedByUsers { get; set; } = new List<SecRole>();

    public virtual SecUserAuth? SecUserAuth { get; set; }

    public virtual ICollection<SecUserManager> SecUserManagerManagers { get; set; } = new List<SecUserManager>();

    public virtual ICollection<SecUserManager> SecUserManagerUsers { get; set; } = new List<SecUserManager>();

    public virtual ICollection<SecUserRole> SecUserRoleCreatedByUsers { get; set; } = new List<SecUserRole>();

    public virtual ICollection<SecUserRole> SecUserRoleModifiedByUsers { get; set; } = new List<SecUserRole>();

    public virtual ICollection<SecUserRole> SecUserRoleUsers { get; set; } = new List<SecUserRole>();
}

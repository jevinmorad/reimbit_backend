namespace Reimbit.Domain.Models;

public partial class SecUser
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? MobileNo { get; set; }

    public string? UserProfileImageUrl { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual ICollection<ComExpenseQuery> ComExpenseQueries { get; set; } = new List<ComExpenseQuery>();

    public virtual SecUser? CreatedByUser { get; set; } = null!;

    public virtual ICollection<ExpCategory> ExpCategoryCreatedByUsers { get; set; } = new List<ExpCategory>();

    public virtual ICollection<ExpCategory> ExpCategoryModifiedByUsers { get; set; } = new List<ExpCategory>();

    public virtual ICollection<ExpExpense> ExpExpenseCreatedByUsers { get; set; } = new List<ExpExpense>();

    public virtual ICollection<ExpExpense> ExpExpenseModifiedByUsers { get; set; } = new List<ExpExpense>();

    public virtual ICollection<ExpExpense> ExpExpenseUsers { get; set; } = new List<ExpExpense>();

    public virtual ICollection<ExpReport> ExpReportCreatedByUsers { get; set; } = new List<ExpReport>();

    public virtual ICollection<ExpReport> ExpReportManagers { get; set; } = new List<ExpReport>();

    public virtual ICollection<ExpReport> ExpReportModifiedByUsers { get; set; } = new List<ExpReport>();

    public virtual ICollection<SecUser> InverseCreatedByUser { get; set; } = new List<SecUser>();

    public virtual ICollection<SecUser> InverseModifiedByUser { get; set; } = new List<SecUser>();

    public virtual SecUser? ModifiedByUser { get; set; } = null!;

    public virtual ICollection<OrgOrganization> OrgOrganizationCreatedByUsers { get; set; } = new List<OrgOrganization>();

    public virtual ICollection<OrgOrganization> OrgOrganizationModifiedByUsers { get; set; } = new List<OrgOrganization>();

    public virtual ICollection<ProjProject> ProjProjectCreatedByUsers { get; set; } = new List<ProjProject>();

    public virtual ICollection<ProjProject> ProjProjectManagers { get; set; } = new List<ProjProject>();

    public virtual ICollection<ProjProjectMember> ProjProjectMembers { get; set; } = new List<ProjProjectMember>();

    public virtual ICollection<ProjProject> ProjProjectModifiedByUsers { get; set; } = new List<ProjProject>();

    public virtual ICollection<SecRoleClaim> SecRoleClaimCreatedByUsers { get; set; } = new List<SecRoleClaim>();

    public virtual ICollection<SecRoleClaim> SecRoleClaimModifiedByUsers { get; set; } = new List<SecRoleClaim>();

    public virtual ICollection<SecRole> SecRoleCreatedByUsers { get; set; } = new List<SecRole>();

    public virtual ICollection<SecRole> SecRoleModifiedByUsers { get; set; } = new List<SecRole>();

    public virtual SecUserAuth? SecUserAuth { get; set; }

    public virtual ICollection<SecUserRole> SecUserRoleCreatedByUsers { get; set; } = new List<SecUserRole>();

    public virtual ICollection<SecUserRole> SecUserRoleModifiedByUsers { get; set; } = new List<SecUserRole>();

    public virtual ICollection<SecUserRole> SecUserRoleUsers { get; set; } = new List<SecUserRole>();
}

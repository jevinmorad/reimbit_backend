using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class OrgOrganization
{
    public int OrganizationId { get; set; }

    public string OrganizationName { get; set; } = null!;

    public int CreatedByUserId { get; set; }

    public int ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual ICollection<ExpCategory> ExpCategories { get; set; } = new List<ExpCategory>();

    public virtual ICollection<ExpExpense> ExpExpenses { get; set; } = new List<ExpExpense>();

    public virtual ICollection<ExpReport> ExpReports { get; set; } = new List<ExpReport>();

    public virtual SecUser ModifiedByUser { get; set; } = null!;

    public virtual ICollection<ProjProjectMember> ProjProjectMembers { get; set; } = new List<ProjProjectMember>();

    public virtual ICollection<ProjProject> ProjProjects { get; set; } = new List<ProjProject>();

    public virtual ICollection<SecRole> SecRoles { get; set; } = new List<SecRole>();

    public virtual ICollection<SecUserAuth> SecUserAuth { get; set; } = new List<SecUserAuth>();
}

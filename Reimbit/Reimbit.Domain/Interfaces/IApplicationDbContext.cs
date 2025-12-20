using Microsoft.EntityFrameworkCore;
using Reimbit.Domain.Models;

namespace Reimbit.Domain.Interfaces;

public interface IApplicationDbContext
{
    DbSet<SecUser> SecUsers { get; }
    DbSet<SecUserAuth> SecUserAuths { get; }
    DbSet<SecUserRole> SecUserRoles { get; }
    DbSet<SecRole> SecRoles { get; }
    DbSet<SecRoleClaim> SecRoleClaims { get; }
    DbSet<SecOperation> SecOperations { get; }

    DbSet<OrgOrganization> OrgOrganizations { get; }
    DbSet<ProjProject> ProjProjects { get; }
    DbSet<ProjProjectMember> ProjProjectMembers { get; }
    DbSet<ProjExpensePolicy> ProjExpensePolicies { get; }

    DbSet<ExpCategory> ExpCategories { get; }
    DbSet<ExpExpense> ExpExpenses { get; }
    DbSet<ExpReport> ExpReports { get; }
    DbSet<ComExpenseQuery> ComExpenseQueries { get; }

    DbSet<LogComExpenseQuery> LogComExpenseQueries { get; }
    DbSet<LogErrorDbm> LogErrorDbms { get; }
    DbSet<LogExpCategory> LogExpCategories { get; }
    DbSet<LogExpExpense> LogExpExpenses { get; }
    DbSet<LogExpReport> LogExpReports { get; }
    DbSet<LogOrgOrganization> LogOrgOrganizations { get; }
    DbSet<LogProjProject> LogProjProjects { get; }
    DbSet<LogProjProjectMember> LogProjProjectMembers { get; }
    DbSet<LogSecRoleClaim> LogSecRoleClaims { get; }
    DbSet<LogSecUser> LogSecUsers { get; }
    DbSet<LogSecUserAuth> LogSecUserAuths { get; }
    DbSet<LogSecUserRole> LogSecUserRoles { get; }

    DbSet<MstSpexecution> MstSpexecutions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}

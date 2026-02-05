using Microsoft.EntityFrameworkCore;
using Reimbit.Domain.Models;

namespace Reimbit.Domain.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AprApprovalInstance> AprApprovalInstances { get; set; }

    DbSet<AprApprovalLevel> AprApprovalLevels { get; set; }

    DbSet<AprApprovalRule> AprApprovalRules { get; set; }

    DbSet<ExpCategory> ExpCategories { get; set; }

    DbSet<ExpExpense> ExpExpenses { get; set; }

    DbSet<ExpExpenseRejection> ExpExpenseRejections { get; set; }

    DbSet<ExpExpenseReport> ExpExpenseReports { get; set; }

    DbSet<ExpPolicy> ExpPolicies { get; set; }

    DbSet<ExpReportExpense> ExpReportExpenses { get; set; }

    DbSet<ExpReportRejection> ExpReportRejections { get; set; }

    DbSet<OrgOrganization> OrgOrganizations { get; set; }

    DbSet<PayPayout> PayPayouts { get; set; }

    DbSet<SecDelegateApprover> SecDelegateApprovers { get; set; }

    DbSet<SecRole> SecRoles { get; set; }

    DbSet<SecRoleClaim> SecRoleClaims { get; set; }

    DbSet<SecUser> SecUsers { get; set; }

    DbSet<SecUserAuth> SecUserAuths { get; set; }

    DbSet<SecUserManager> SecUserManagers { get; set; }

    DbSet<SecUserRole> SecUserRoles { get; set; }

    DbSet<SysAuditLog> SysAuditLogs { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}

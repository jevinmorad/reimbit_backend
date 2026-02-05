using AegisInt.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;

namespace Reimbit.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.UseAegisIntEncryption();
    }

    public virtual DbSet<AprApprovalInstance> AprApprovalInstances { get; set; }

    public virtual DbSet<AprApprovalLevel> AprApprovalLevels { get; set; }

    public virtual DbSet<AprApprovalRule> AprApprovalRules { get; set; }

    public virtual DbSet<ExpCategory> ExpCategories { get; set; }

    public virtual DbSet<ExpExpense> ExpExpenses { get; set; }

    public virtual DbSet<ExpExpenseRejection> ExpExpenseRejections { get; set; }

    public virtual DbSet<ExpExpenseReport> ExpExpenseReports { get; set; }

    public virtual DbSet<ExpPolicy> ExpPolicies { get; set; }

    public virtual DbSet<ExpReportExpense> ExpReportExpenses { get; set; }

    public virtual DbSet<ExpReportRejection> ExpReportRejections { get; set; }

    public virtual DbSet<OrgOrganization> OrgOrganizations { get; set; }

    public virtual DbSet<PayPayout> PayPayouts { get; set; }

    public virtual DbSet<SecDelegateApprover> SecDelegateApprovers { get; set; }

    public virtual DbSet<SecRole> SecRoles { get; set; }

    public virtual DbSet<SecRoleClaim> SecRoleClaims { get; set; }

    public virtual DbSet<SecUser> SecUsers { get; set; }

    public virtual DbSet<SecUserAuth> SecUserAuths { get; set; }

    public virtual DbSet<SecUserManager> SecUserManagers { get; set; }

    public virtual DbSet<SecUserRole> SecUserRoles { get; set; }

    public virtual DbSet<SysAuditLog> SysAuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AprApprovalInstance>(entity =>
        {
            entity.HasKey(e => e.ApprovalInstanceId).HasName("PK__APR_Appr__6F184C7300081207");

            entity.ToTable("APR_ApprovalInstance");

            entity.Property(e => e.ApprovalInstanceId).HasColumnName("ApprovalInstanceID");
            entity.Property(e => e.ActionAt).HasColumnType("datetime");
            entity.Property(e => e.ApprovalLevelId).HasColumnName("ApprovalLevelID");
            entity.Property(e => e.ApproverUserId).HasColumnName("ApproverUserID");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);

            entity.HasOne(d => d.ApprovalLevel).WithMany(p => p.AprApprovalInstances)
                .HasForeignKey(d => d.ApprovalLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APR_Appro__Appro__47A6A41B");

            entity.HasOne(d => d.ApproverUser).WithMany(p => p.AprApprovalInstances)
                .HasForeignKey(d => d.ApproverUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APR_Appro__Appro__489AC854");

            entity.HasOne(d => d.Report).WithMany(p => p.AprApprovalInstances)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APR_Appro__Repor__46B27FE2");
        });

        modelBuilder.Entity<AprApprovalLevel>(entity =>
        {
            entity.HasKey(e => e.ApprovalLevelId).HasName("PK__APR_Appr__A485982A65272F71");

            entity.ToTable("APR_ApprovalLevel");

            entity.Property(e => e.ApprovalLevelId).HasColumnName("ApprovalLevelID");
            entity.Property(e => e.ApprovalRuleId).HasColumnName("ApprovalRuleID");
            entity.Property(e => e.ApproverRoleId).HasColumnName("ApproverRoleID");
            entity.Property(e => e.SpecificUserId).HasColumnName("SpecificUserID");

            entity.HasOne(d => d.ApprovalRule).WithMany(p => p.AprApprovalLevels)
                .HasForeignKey(d => d.ApprovalRuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APR_Appro__Appro__43D61337");
        });

        modelBuilder.Entity<AprApprovalRule>(entity =>
        {
            entity.HasKey(e => e.ApprovalRuleId).HasName("PK__APR_Appr__1D43072908CCF343");

            entity.ToTable("APR_ApprovalRule");

            entity.Property(e => e.ApprovalRuleId).HasColumnName("ApprovalRuleID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaxAmount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.MinAmount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.RuleName).HasMaxLength(150);

            entity.HasOne(d => d.Organization).WithMany(p => p.AprApprovalRules)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APR_Appro__Organ__3E1D39E1");
        });

        modelBuilder.Entity<ExpCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__EXP_Cate__19093A2BDE96BD61");

            entity.ToTable("EXP_Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ExpCategoryCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Categ__Creat__1332DBDC");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.ExpCategoryModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Categ__Modif__14270015");

            entity.HasOne(d => d.Organization).WithMany(p => p.ExpCategories)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Categ__Organ__114A936A");
        });

        modelBuilder.Entity<ExpExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__EXP_Expe__1445CFF3F0C86723");

            entity.ToTable("EXP_Expense");

            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("INR");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ReceiptUrl).HasMaxLength(25);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Category).WithMany(p => p.ExpExpenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Categ__2180FB33");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ExpExpenseCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Creat__245D67DE");

            entity.HasOne(d => d.Employee).WithMany(p => p.ExpExpenseEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Emplo__1F98B2C1");

            entity.HasOne(d => d.Organization).WithMany(p => p.ExpExpenses)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Organ__1EA48E88");
        });

        modelBuilder.Entity<ExpExpenseRejection>(entity =>
        {
            entity.HasKey(e => e.ExpenseRejectionId).HasName("PK__EXP_Expe__3E9E1C055D8032CD");

            entity.ToTable("EXP_ExpenseRejection");

            entity.Property(e => e.ExpenseRejectionId).HasColumnName("ExpenseRejectionID");
            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.RejectedAt)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.RejectedByUserId).HasColumnName("RejectedByUserID");

            entity.HasOne(d => d.Expense).WithMany(p => p.ExpExpenseRejections)
                .HasForeignKey(d => d.ExpenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Expen__3493CFA7");

            entity.HasOne(d => d.RejectedByUser).WithMany(p => p.ExpExpenseRejections)
                .HasForeignKey(d => d.RejectedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Rejec__3587F3E0");
        });

        modelBuilder.Entity<ExpExpenseReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__EXP_Expe__D5BD48E5976605FF");

            entity.ToTable("EXP_ExpenseReport");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ViewedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ExpExpenseReportCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Creat__2B0A656D");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.ExpExpenseReportModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Modif__2BFE89A6");

            entity.HasOne(d => d.Organization).WithMany(p => p.ExpExpenseReports)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Organ__282DF8C2");
        });

        modelBuilder.Entity<ExpPolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__EXP_Poli__2E1339446B2BB7D3");

            entity.ToTable("EXP_Policy");

            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.MaxAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

            entity.HasOne(d => d.Category).WithMany(p => p.ExpPolicies)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__EXP_Polic__Categ__17F790F9");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ExpPolicyCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Polic__Creat__19DFD96B");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.ExpPolicyModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Polic__Modif__1AD3FDA4");
        });

        modelBuilder.Entity<ExpReportExpense>(entity =>
        {
            entity.HasKey(e => e.ReportExpenseId).HasName("PK__EXP_Repo__020DDA30B72D2457");

            entity.ToTable("EXP_ReportExpense");

            entity.HasIndex(e => new { e.ReportId, e.ExpenseId }, "UQ_ReportExpense").IsUnique();

            entity.Property(e => e.ReportExpenseId).HasColumnName("ReportExpenseID");
            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");

            entity.HasOne(d => d.Expense).WithMany(p => p.ExpReportExpenses)
                .HasForeignKey(d => d.ExpenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Expen__31B762FC");

            entity.HasOne(d => d.Report).WithMany(p => p.ExpReportExpenses)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Repor__30C33EC3");
        });

        modelBuilder.Entity<ExpReportRejection>(entity =>
        {
            entity.HasKey(e => e.ReportRejectionId).HasName("PK__EXP_Repo__9F16AE6598203DD5");

            entity.ToTable("EXP_ReportRejection");

            entity.Property(e => e.ReportRejectionId).HasColumnName("ReportRejectionID");
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.RejectedAt)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.RejectedByUserId).HasColumnName("RejectedByUserID");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");

            entity.HasOne(d => d.RejectedByUser).WithMany(p => p.ExpReportRejections)
                .HasForeignKey(d => d.RejectedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Rejec__3A4CA8FD");

            entity.HasOne(d => d.Report).WithMany(p => p.ExpReportRejections)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Repor__395884C4");
        });

        modelBuilder.Entity<OrgOrganization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("PK__ORG_Orga__CADB0B7275C41587");

            entity.ToTable("ORG_Organization");

            entity.HasIndex(e => e.OrganizationName, "UQ__ORG_Orga__F50959E4B7A6D3E6").IsUnique();

            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrganizationName).HasMaxLength(100);
        });

        modelBuilder.Entity<PayPayout>(entity =>
        {
            entity.HasKey(e => e.PayoutId).HasName("PK__PAY_Payo__35C3DFAE39E67E47");

            entity.ToTable("PAY_Payout");

            entity.Property(e => e.PayoutId).HasColumnName("PayoutID");
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.PaidOn).HasColumnType("datetime");
            entity.Property(e => e.ReferenceNo).HasMaxLength(100);
            entity.Property(e => e.ReportId).HasColumnName("ReportID");

            entity.HasOne(d => d.Report).WithMany(p => p.PayPayouts)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PAY_Payou__Repor__503BEA1C");
        });

        modelBuilder.Entity<SecDelegateApprover>(entity =>
        {
            entity.HasKey(e => e.DelegateId).HasName("PK__SEC_Dele__013A454B65FF5F30");

            entity.ToTable("SEC_DelegateApprover");

            entity.Property(e => e.DelegateId).HasColumnName("DelegateID");
            entity.Property(e => e.DelegateUserId).HasColumnName("DelegateUserID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");

            entity.HasOne(d => d.DelegateUser).WithMany(p => p.SecDelegateApproverDelegateUsers)
                .HasForeignKey(d => d.DelegateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_Deleg__Deleg__4D5F7D71");

            entity.HasOne(d => d.User).WithMany(p => p.SecDelegateApproverUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_Deleg__UserI__4C6B5938");
        });

        modelBuilder.Entity<SecRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__SEC_Role__8AFACE3A31192DF2");

            entity.ToTable("SEC_Role");

            entity.HasIndex(e => new { e.OrganizationId, e.RoleName }, "UK_SEC_Role_Org_Name").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.SecRoleCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_Role__Create__75A278F5");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.SecRoleModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_Role__Modifi__76969D2E");

            entity.HasOne(d => d.Organization).WithMany(p => p.SecRoles)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK__SEC_Role__Organi__74AE54BC");
        });

        modelBuilder.Entity<SecRoleClaim>(entity =>
        {
            entity.HasKey(e => e.RoleClaimId).HasName("PK__SEC_Role__BB90E9762843233E");

            entity.ToTable("SEC_RoleClaim");

            entity.Property(e => e.RoleClaimId).HasColumnName("RoleClaimID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.SecRoleClaims)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_RoleC__Creat__7D439ABD");

            entity.HasOne(d => d.Role).WithMany(p => p.SecRoleClaims)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_RoleC__RoleI__7C4F7684");
        });

        modelBuilder.Entity<SecUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__SEC_User__1788CCAC3EC1AB1D");

            entity.ToTable("SEC_User");

            entity.HasIndex(e => e.Email, "UQ__SEC_User__A9D10534D2002D7E").IsUnique();

            entity.HasIndex(e => e.MobileNo, "UQ__SEC_User__D6D73A86328ED36B").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.DisplayName).HasMaxLength(110);
            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MobileNo).HasMaxLength(50);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.InverseCreatedByUser)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK__SEC_User__Create__6477ECF3");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.InverseModifiedByUser)
                .HasForeignKey(d => d.ModifiedByUserId)
                .HasConstraintName("FK__SEC_User__Modifi__656C112C");

            entity.HasOne(d => d.Organization).WithMany(p => p.SecUsers)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK__SEC_User__Organi__628FA481");
        });

        modelBuilder.Entity<SecUserAuth>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__SEC_User__1788CCACE9656E9A");

            entity.ToTable("SEC_UserAuth");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.RefreshToken).HasMaxLength(250);
            entity.Property(e => e.RefreshTokenExpiry).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.SecUserAuth)
                .HasForeignKey<SecUserAuth>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserA__UserI__693CA210");
        });

        modelBuilder.Entity<SecUserManager>(entity =>
        {
            entity.HasKey(e => e.UserManagerId).HasName("PK__SEC_User__96A0B52D8AD61BCA");

            entity.ToTable("SEC_UserManager");

            entity.HasIndex(e => new { e.UserId, e.ManagerId, e.ManagerType }, "UQ_User_Manager").IsUnique();

            entity.Property(e => e.UserManagerId).HasColumnName("UserManagerID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ValidFrom)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");

            entity.HasOne(d => d.Manager).WithMany(p => p.SecUserManagerManagers)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserM__Manag__6EF57B66");

            entity.HasOne(d => d.User).WithMany(p => p.SecUserManagerUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserM__UserI__6E01572D");
        });

        modelBuilder.Entity<SecUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__SEC_User__3D978A55A35A4583");

            entity.ToTable("SEC_UserRole");

            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.SecUserRoleCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserR__Creat__02084FDA");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.SecUserRoleModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserR__Modif__02FC7413");

            entity.HasOne(d => d.Role).WithMany(p => p.SecUserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserR__RoleI__01142BA1");

            entity.HasOne(d => d.User).WithMany(p => p.SecUserRoleUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserR__UserI__00200768");
        });

        modelBuilder.Entity<SysAuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditLogId).HasName("PK__SYS_Audi__EB5F6CDD4B475BE4");

            entity.ToTable("SYS_AuditLog");

            entity.Property(e => e.AuditLogId).HasColumnName("AuditLogID");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(45)
                .HasColumnName("IPAddress");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.UserAgent).HasMaxLength(300);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });
    }
}
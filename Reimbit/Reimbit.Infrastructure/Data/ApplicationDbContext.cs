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

    // Security
    public DbSet<SecUser> SecUsers { get; set; } = default!;
    public DbSet<SecUserAuth> SecUserAuths { get; set; } = default!;
    public DbSet<SecUserRole> SecUserRoles { get; set; } = default!;
    public DbSet<SecRole> SecRoles { get; set; } = default!;
    public DbSet<SecRoleClaim> SecRoleClaims { get; set; } = default!;
    public DbSet<SecOperation> SecOperations { get; set; } = default!;

    // Organization / Projects
    public DbSet<OrgOrganization> OrgOrganizations { get; set; } = default!;
    public DbSet<ProjProject> ProjProjects { get; set; } = default!;
    public DbSet<ProjProjectMember> ProjProjectMembers { get; set; } = default!;
    public DbSet<ProjExpensePolicy> ProjExpensePolicies { get; set; } = default!;

    // Expenses
    public DbSet<ExpCategory> ExpCategories { get; set; } = default!;
    public DbSet<ExpExpense> ExpExpenses { get; set; } = default!;
    public DbSet<ExpReport> ExpReports { get; set; } = default!;
    public DbSet<ComExpenseQuery> ComExpenseQueries { get; set; } = default!;

    // Logs
    public DbSet<LogComExpenseQuery> LogComExpenseQueries { get; set; } = default!;
    public DbSet<LogErrorDbm> LogErrorDbms { get; set; } = default!;
    public DbSet<LogExpCategory> LogExpCategories { get; set; } = default!;
    public DbSet<LogExpExpense> LogExpExpenses { get; set; } = default!;
    public DbSet<LogExpReport> LogExpReports { get; set; } = default!;
    public DbSet<LogOrgOrganization> LogOrgOrganizations { get; set; } = default!;
    public DbSet<LogProjProject> LogProjProjects { get; set; } = default!;
    public DbSet<LogProjProjectMember> LogProjProjectMembers { get; set; } = default!;
    public DbSet<LogSecRoleClaim> LogSecRoleClaims { get; set; } = default!;
    public DbSet<LogSecUser> LogSecUsers { get; set; } = default!;
    public DbSet<LogSecUserAuth> LogSecUserAuths { get; set; } = default!;
    public DbSet<LogSecUserRole> LogSecUserRoles { get; set; } = default!;

    // Misc
    public DbSet<MstSpexecution> MstSpexecutions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ComExpenseQuery>(entity =>
        {
            entity.HasKey(e => e.QueryId).HasName("PK__COM_Expe__5967F7FBBC815B6A");

            entity.ToTable("COM_ExpenseQuery");

            entity.Property(e => e.QueryId).HasColumnName("QueryID");
            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(1000);
            entity.Property(e => e.SenderUserId).HasColumnName("SenderUserID");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Expense).WithMany(p => p.ComExpenseQueries)
                .HasForeignKey(d => d.ExpenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COM_Expen__Expen__3864608B");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.ComExpenseQueries)
                .HasForeignKey(d => d.SenderUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COM_Expen__Sende__395884C4");
        });

        modelBuilder.Entity<ExpCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__EXP_Cate__19093A2B34B6A9B7");

            entity.ToTable("EXP_Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ExpCategoryCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Categ__Creat__1F98B2C1");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.ExpCategoryModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .HasConstraintName("FK__EXP_Categ__Modif__208CD6FA");

            entity.HasOne(d => d.Organization).WithMany(p => p.ExpCategories)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Categ__Organ__1DB06A4F");

            entity.HasOne(d => d.Project).WithMany(p => p.ExpCategories)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Categ__Proje__1EA48E88");
        });

        modelBuilder.Entity<ExpExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__EXP_Expe__1445CFF34223D76F");

            entity.ToTable("EXP_Expense");

            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.AttachmentUrl).HasMaxLength(25);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("INR");
            entity.Property(e => e.ExpenseStatus)
                .HasMaxLength(30)
                .HasDefaultValue("submitted");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.ExpExpenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Categ__2EDAF651");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ExpExpenseCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Creat__31B762FC");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.ExpExpenseModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Modif__32AB8735");

            entity.HasOne(d => d.Organization).WithMany(p => p.ExpExpenses)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Organ__2BFE89A6");

            entity.HasOne(d => d.Project).WithMany(p => p.ExpExpenses)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__Proje__2DE6D218");

            entity.HasOne(d => d.User).WithMany(p => p.ExpExpenseUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Expen__UserI__2CF2ADDF");
        });

        modelBuilder.Entity<ExpReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__EXP_Repo__D5BD48E52D8C23FB");

            entity.ToTable("EXP_Report");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.AcceptedAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.RejectedAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ReportStatus)
                .HasMaxLength(30)
                .HasDefaultValue("submitted");
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.ViewedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ExpReportCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Creat__45BE5BA9");

            entity.HasOne(d => d.Manager).WithMany(p => p.ExpReportManagers)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Manag__40F9A68C");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.ExpReportModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Modif__46B27FE2");

            entity.HasOne(d => d.Organization).WithMany(p => p.ExpReports)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Organ__40058253");

            entity.HasOne(d => d.Project).WithMany(p => p.ExpReports)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXP_Repor__Proje__41EDCAC5");
        });

        modelBuilder.Entity<LogComExpenseQuery>(entity =>
        {
            entity.HasKey(e => e.LogexpenseId).HasName("PK__LOG_COM___1396EC433B74A47F");

            entity.ToTable("LOG_COM_ExpenseQuery");

            entity.Property(e => e.LogexpenseId).HasColumnName("LOGExpenseID");
            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.Message).HasMaxLength(1000);
            entity.Property(e => e.QueryId).HasColumnName("QueryID");
            entity.Property(e => e.SenderUserId).HasColumnName("SenderUserID");
            entity.Property(e => e.SentAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<LogErrorDbm>(entity =>
        {
            entity.HasKey(e => e.ErrorDbmsid).HasName("PK__LOG_Erro__C97849796A38A933");

            entity.ToTable("LOG_ErrorDBMS");

            entity.Property(e => e.ErrorDbmsid).HasColumnName("ErrorDBMSID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(4000);
            entity.Property(e => e.ErrorProcedure).HasMaxLength(4000);
            entity.Property(e => e.Spdetail)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("SPDetail");
            entity.Property(e => e.VerifiedOn).HasColumnType("datetime");
            entity.Property(e => e.VerifiedRemarks).HasMaxLength(1000);
        });

        modelBuilder.Entity<LogExpCategory>(entity =>
        {
            entity.HasKey(e => e.LogcategoryId).HasName("PK__LOG_EXP___826DC51EFF8B6848");

            entity.ToTable("LOG_EXP_Category");

            entity.Property(e => e.LogcategoryId).HasColumnName("LOGCategoryID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
        });

        modelBuilder.Entity<LogExpExpense>(entity =>
        {
            entity.HasKey(e => e.LogexpenseId).HasName("PK__LOG_EXP___1396EC43F90D78B8");

            entity.ToTable("LOG_EXP_Expense");

            entity.Property(e => e.LogexpenseId).HasColumnName("LOGExpenseID");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.AttachmentUrl).HasMaxLength(25);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.ExpenseStatus).HasMaxLength(30);
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<LogExpReport>(entity =>
        {
            entity.HasKey(e => e.LogorganizationId).HasName("PK__LOG_EXP___069D042F58DD22DF");

            entity.ToTable("LOG_EXP_Report");

            entity.Property(e => e.LogorganizationId).HasColumnName("LOGOrganizationID");
            entity.Property(e => e.AcceptedAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.RejectedAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.ReportStatus).HasMaxLength(30);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.ViewedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<LogOrgOrganization>(entity =>
        {
            entity.HasKey(e => e.LogorganizationId).HasName("PK__LOG_ORG___069D042F86518407");

            entity.ToTable("LOG_ORG_Organization");

            entity.Property(e => e.LogorganizationId).HasColumnName("LOGOrganizationID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.OrganizationName).HasMaxLength(100);
        });

        modelBuilder.Entity<LogProjProject>(entity =>
        {
            entity.HasKey(e => e.LogprojectsId).HasName("PK__LOG_PROJ__FFB6CA576D4F012F");

            entity.ToTable("LOG_PROJ_Project");

            entity.Property(e => e.LogprojectsId).HasColumnName("LOGProjectsID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectDescription).HasMaxLength(500);
            entity.Property(e => e.ProjectDetails).HasMaxLength(500);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.ProjectLogoUrl).HasMaxLength(250);
            entity.Property(e => e.ProjectName).HasMaxLength(250);
        });

        modelBuilder.Entity<LogProjProjectMember>(entity =>
        {
            entity.HasKey(e => e.LogprojectMemberId).HasName("PK__LOG_PROJ__73616FE1CC96DD33");

            entity.ToTable("LOG_PROJ_ProjectMember");

            entity.Property(e => e.LogprojectMemberId).HasColumnName("LOGProjectMemberID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<LogSecRoleClaim>(entity =>
        {
            entity.HasKey(e => e.LogorganizationId).HasName("PK__LOG_SEC___069D042FD03431A3");

            entity.ToTable("LOG_SEC_RoleClaim");

            entity.Property(e => e.LogorganizationId).HasColumnName("LOGOrganizationID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.RoleClaimId).HasColumnName("RoleClaimID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
        });

        modelBuilder.Entity<LogSecUser>(entity =>
        {
            entity.HasKey(e => e.LogUserId).HasName("PK__LOG_SEC___96549365657C309E");

            entity.ToTable("LOG_SEC_User");

            entity.Property(e => e.LogUserId).HasColumnName("LOGUserID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.MobileNo).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(150);
            entity.Property(e => e.UserProfileImageUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<LogSecUserAuth>(entity =>
        {
            entity.HasKey(e => e.LoguserAuthId).HasName("PK__LOG_SEC___5AAC1518BAB4BDB6");

            entity.ToTable("LOG_SEC_UserAuth");

            entity.Property(e => e.LoguserAuthId).HasColumnName("LOGUserAuthID");
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.OrganizationId).HasColumnType("OrganizationID");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.RefreshToken).HasMaxLength(250);
            entity.Property(e => e.RefreshTokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<LogSecUserRole>(entity =>
        {
            entity.HasKey(e => e.LogorganizationId).HasName("PK__LOG_SEC___069D042FC77CE1D9");

            entity.ToTable("LOG_SEC_UserRole");

            entity.Property(e => e.LogorganizationId).HasColumnName("LOGOrganizationID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Iud)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IUD");
            entity.Property(e => e.IudbyUserId).HasColumnName("IUDByUserID");
            entity.Property(e => e.IuddateTime)
                .HasColumnType("datetime")
                .HasColumnName("IUDDateTime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
        });

        modelBuilder.Entity<MstSpexecution>(entity =>
        {
            entity.HasKey(e => e.SpexecutionId).HasName("PK__MST_SPEx__78733736095011A3");

            entity.ToTable("MST_SPExecution");

            entity.Property(e => e.SpexecutionId).HasColumnName("SPExecutionID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.ExecutionTimeMs).HasColumnName("ExecutionTimeMS");
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.Spname)
                .HasMaxLength(200)
                .HasColumnName("SPName");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<OrgOrganization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("PK__ORG_Orga__CADB0B722E44CF29");

            entity.ToTable("ORG_Organization");

            entity.HasIndex(e => e.OrganizationName, "UQ__ORG_Orga__F50959E41C5632A3").IsUnique();

            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationName).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.OrgOrganizationCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ORG_Organ__Creat__6A30C649");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.OrgOrganizationModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ORG_Organ__Modif__6B24EA82");
        });

        modelBuilder.Entity<ProjExpensePolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__PROJ_Exp__2E13394431904DD5");

            entity.ToTable("PROJ_ExpensePolicy");

            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.MaxAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.Category).WithMany(p => p.ProjExpensePolicies)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__PROJ_Expe__Categ__2739D489");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjExpensePolicies)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJ_Expe__Proje__2645B050");
        });

        modelBuilder.Entity<ProjProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__PROJ_Pro__761ABED07EB7813A");

            entity.ToTable("PROJ_Project");

            entity.HasIndex(e => new { e.ProjectName, e.OrganizationId }, "PROJ_Project_ORG").IsUnique();

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectDescription).HasMaxLength(500);
            entity.Property(e => e.ProjectDetails).HasMaxLength(500);
            entity.Property(e => e.ProjectLogoUrl).HasMaxLength(250);
            entity.Property(e => e.ProjectName).HasMaxLength(250);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ProjProjectCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJ_Proj__Creat__0E6E26BF");

            entity.HasOne(d => d.Manager).WithMany(p => p.ProjProjectManagers)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJ_Proj__Manag__0D7A0286");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.ProjProjectModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJ_Proj__Modif__0F624AF8");

            entity.HasOne(d => d.Organization).WithMany(p => p.ProjProjects)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJ_Proj__Organ__0C85DE4D");
        });

        modelBuilder.Entity<ProjProjectMember>(entity =>
        {
            entity.HasKey(e => e.ProjectMemberId).HasName("PK__PROJ_Pro__E4E9983C3384D229");

            entity.ToTable("PROJ_ProjectMember");

            entity.Property(e => e.ProjectMemberId).HasColumnName("ProjectMemberID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Organization).WithMany(p => p.ProjProjectMembers)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJ_Proj__Organ__17036CC0");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjProjectMembers)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJ_Proj__Proje__160F4887");

            entity.HasOne(d => d.User).WithMany(p => p.ProjProjectMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PROJ_Proj__UserI__17F790F9");
        });

        modelBuilder.Entity<SecOperation>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PK__SEC_Oper__A4F5FC6459503172");

            entity.ToTable("SEC_Operation");

            entity.Property(e => e.OperationId)
                .ValueGeneratedNever()
                .HasColumnName("OperationID");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.OperationName).HasMaxLength(250);
        });

        modelBuilder.Entity<SecRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__SEC_Role__8AFACE3A5F88263D");

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

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.SecRoleCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_Role__Create__778AC167");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.SecRoleModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_Role__Modifi__787EE5A0");

            entity.HasOne(d => d.Organization).WithMany(p => p.SecRoles)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK__SEC_Role__Organi__76969D2E");
        });

        modelBuilder.Entity<SecRoleClaim>(entity =>
        {
            entity.HasKey(e => e.RoleClaimId).HasName("PK__SEC_Role__BB90E97636D64882");

            entity.ToTable("SEC_RoleClaim");

            entity.Property(e => e.RoleClaimId).HasColumnName("RoleClaimID");
            entity.Property(e => e.ClaimValue).HasMaxLength(200);
            entity.Property(e => e.ClaminType).HasMaxLength(200);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.SecRoleClaimCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_RoleC__Creat__7D439ABD");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.SecRoleClaimModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_RoleC__Modif__7E37BEF6");

            entity.HasOne(d => d.Role).WithMany(p => p.SecRoleClaims)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_RoleC__RoleI__7C4F7684");
        });

        modelBuilder.Entity<SecUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__SEC_User__1788CCAC69E512E5");

            entity.ToTable("SEC_User");

            entity.HasIndex(e => e.Email, "UQ__SEC_User__08638DF858550AC5").IsUnique();

            entity.HasIndex(e => e.MobileNo, "UQ__SEC_User__1D6F012637269705").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.MobileNo).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(150);
            entity.Property(e => e.UserProfileImageUrl).HasMaxLength(500);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.InverseCreatedByUser)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_User__Create__628FA481");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.InverseModifiedByUser)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_User__Modifi__6383C8BA");
        });

        modelBuilder.Entity<SecUserAuth>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__SEC_User__1788CCACC0680279");

            entity.ToTable("SEC_UserAuth");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.RefreshToken).HasMaxLength(250);
            entity.Property(e => e.RefreshTokenExpiry).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.SecUserAuth)
                .HasForeignKey<SecUserAuth>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserA__UserI__70DDC3D8");

            entity.HasOne(d => d.Organization).WithMany(p => p.SecUserAuth)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserA__Organ__607251E5");
        });

        modelBuilder.Entity<SecUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__SEC_User__3D978A55BD9594BD");

            entity.ToTable("SEC_UserRole");

            entity.Property(e => e.UserRoleId)
                .ValueGeneratedNever()
                .HasColumnName("UserRoleID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("([dbo].[GETServerDateTime]())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.SecUserRoleCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserR__Creat__04E4BC85");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.SecUserRoleModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SEC_UserR__Modif__05D8E0BE");

            entity.HasOne(d => d.User).WithMany(p => p.SecUserRoleUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SEC_UserR__UserI__03F0984C");
        });
    }
}
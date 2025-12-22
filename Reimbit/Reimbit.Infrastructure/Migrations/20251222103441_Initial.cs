using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reimbit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOG_COM_ExpenseQuery",
                columns: table => new
                {
                    LOGExpenseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    QueryID = table.Column<int>(type: "int", nullable: true),
                    ExpenseID = table.Column<int>(type: "int", nullable: true),
                    SenderUserID = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_COM___1396EC433B74A47F", x => x.LOGExpenseID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_ErrorDBMS",
                columns: table => new
                {
                    ErrorDBMSID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SPDetail = table.Column<string>(type: "varchar(4000)", unicode: false, maxLength: 4000, nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ErrorProcedure = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ErrorSeverity = table.Column<int>(type: "int", nullable: true),
                    ErrorState = table.Column<int>(type: "int", nullable: true),
                    ErrorLine = table.Column<int>(type: "int", nullable: true),
                    ErrorNumber = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    VerifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    VerifiedRemarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsSolved = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_Erro__C97849796A38A933", x => x.ErrorDBMSID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_EXP_Category",
                columns: table => new
                {
                    LOGCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    OrganizationID = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProjectID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_EXP___826DC51EFF8B6848", x => x.LOGCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_EXP_Expense",
                columns: table => new
                {
                    LOGExpenseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    ExpenseID = table.Column<int>(type: "int", nullable: true),
                    OrganizationID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ProjectID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpenseStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_EXP___1396EC43F90D78B8", x => x.LOGExpenseID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_EXP_Report",
                columns: table => new
                {
                    LOGOrganizationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    ReportID = table.Column<int>(type: "int", nullable: true),
                    OrganizationID = table.Column<int>(type: "int", nullable: true),
                    ManagerID = table.Column<int>(type: "int", nullable: true),
                    ProjectID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReportStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AcceptedAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    RejectedAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ViewedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_EXP___069D042F58DD22DF", x => x.LOGOrganizationID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_ORG_Organization",
                columns: table => new
                {
                    LOGOrganizationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    OrganizationID = table.Column<int>(type: "int", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_ORG___069D042F86518407", x => x.LOGOrganizationID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_PROJ_Project",
                columns: table => new
                {
                    LOGProjectsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProjectLogoUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProjectDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProjectDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrganizationID = table.Column<int>(type: "int", nullable: true),
                    ManagerID = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_PROJ__FFB6CA576D4F012F", x => x.LOGProjectsID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_PROJ_ProjectMember",
                columns: table => new
                {
                    LOGProjectMemberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    OrganizationID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_PROJ__73616FE1CC96DD33", x => x.LOGProjectMemberID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_SEC_RoleClaim",
                columns: table => new
                {
                    LOGOrganizationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    RoleClaimID = table.Column<int>(type: "int", nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_SEC___069D042FD03431A3", x => x.LOGOrganizationID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_SEC_User",
                columns: table => new
                {
                    LOGUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_SEC___96549365657C309E", x => x.LOGUserID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_SEC_UserAuth",
                columns: table => new
                {
                    LOGUserAuthID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_SEC___5AAC1518BAB4BDB6", x => x.LOGUserAuthID);
                });

            migrationBuilder.CreateTable(
                name: "LOG_SEC_UserRole",
                columns: table => new
                {
                    LOGOrganizationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IUD = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    IUDDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUDByUserID = table.Column<int>(type: "int", nullable: false),
                    UserRoleID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOG_SEC___069D042FC77CE1D9", x => x.LOGOrganizationID);
                });

            migrationBuilder.CreateTable(
                name: "MST_SPExecution",
                columns: table => new
                {
                    SPExecutionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SPName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExecutionTimeMS = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MST_SPEx__78733736095011A3", x => x.SPExecutionID);
                });

            migrationBuilder.CreateTable(
                name: "SEC_Operation",
                columns: table => new
                {
                    OperationID = table.Column<int>(type: "int", nullable: false),
                    OperationNo = table.Column<int>(type: "int", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SEC_Oper__A4F5FC6459503172", x => x.OperationID);
                });

            migrationBuilder.CreateTable(
                name: "SEC_User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SEC_User__1788CCAC69E512E5", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__SEC_User__Create__628FA481",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__SEC_User__Modifi__6383C8BA",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ORG_Organization",
                columns: table => new
                {
                    OrganizationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ORG_Orga__CADB0B722E44CF29", x => x.OrganizationID);
                    table.ForeignKey(
                        name: "FK__ORG_Organ__Creat__6A30C649",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__ORG_Organ__Modif__6B24EA82",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "SEC_UserRole",
                columns: table => new
                {
                    UserRoleID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SEC_User__3D978A55BD9594BD", x => x.UserRoleID);
                    table.ForeignKey(
                        name: "FK__SEC_UserR__Creat__04E4BC85",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__SEC_UserR__Modif__05D8E0BE",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__SEC_UserR__UserI__03F0984C",
                        column: x => x.UserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PROJ_Project",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProjectLogoUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProjectDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProjectDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrganizationID = table.Column<int>(type: "int", nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PROJ_Pro__761ABED07EB7813A", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK__PROJ_Proj__Creat__0E6E26BF",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PROJ_Proj__Manag__0D7A0286",
                        column: x => x.ManagerID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PROJ_Proj__Modif__0F624AF8",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__PROJ_Proj__Organ__0C85DE4D",
                        column: x => x.OrganizationID,
                        principalTable: "ORG_Organization",
                        principalColumn: "OrganizationID");
                });

            migrationBuilder.CreateTable(
                name: "SEC_Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrganizationID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SEC_Role__8AFACE3A5F88263D", x => x.RoleID);
                    table.ForeignKey(
                        name: "FK__SEC_Role__Create__778AC167",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__SEC_Role__Modifi__787EE5A0",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__SEC_Role__Organi__76969D2E",
                        column: x => x.OrganizationID,
                        principalTable: "ORG_Organization",
                        principalColumn: "OrganizationID");
                });

            migrationBuilder.CreateTable(
                name: "SEC_UserAuth",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    OrganizationID = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SEC_User__1788CCACC0680279", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__SEC_UserA__Organ__607251E5",
                        column: x => x.OrganizationID,
                        principalTable: "ORG_Organization",
                        principalColumn: "OrganizationID");
                    table.ForeignKey(
                        name: "FK__SEC_UserA__UserI__70DDC3D8",
                        column: x => x.UserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "EXP_Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationID = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EXP_Cate__19093A2B34B6A9B7", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK__EXP_Categ__Creat__1F98B2C1",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__EXP_Categ__Modif__208CD6FA",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__EXP_Categ__Organ__1DB06A4F",
                        column: x => x.OrganizationID,
                        principalTable: "ORG_Organization",
                        principalColumn: "OrganizationID");
                    table.ForeignKey(
                        name: "FK__EXP_Categ__Proje__1EA48E88",
                        column: x => x.ProjectID,
                        principalTable: "PROJ_Project",
                        principalColumn: "ProjectID");
                });

            migrationBuilder.CreateTable(
                name: "EXP_Report",
                columns: table => new
                {
                    ReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationID = table.Column<int>(type: "int", nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ReportStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "submitted"),
                    AcceptedAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    RejectedAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EXP_Repo__D5BD48E52D8C23FB", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK__EXP_Repor__Creat__45BE5BA9",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__EXP_Repor__Manag__40F9A68C",
                        column: x => x.ManagerID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__EXP_Repor__Modif__46B27FE2",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__EXP_Repor__Organ__40058253",
                        column: x => x.OrganizationID,
                        principalTable: "ORG_Organization",
                        principalColumn: "OrganizationID");
                    table.ForeignKey(
                        name: "FK__EXP_Repor__Proje__41EDCAC5",
                        column: x => x.ProjectID,
                        principalTable: "PROJ_Project",
                        principalColumn: "ProjectID");
                });

            migrationBuilder.CreateTable(
                name: "PROJ_ProjectMember",
                columns: table => new
                {
                    ProjectMemberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    OrganizationID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PROJ_Pro__E4E9983C3384D229", x => x.ProjectMemberID);
                    table.ForeignKey(
                        name: "FK__PROJ_Proj__Organ__17036CC0",
                        column: x => x.OrganizationID,
                        principalTable: "ORG_Organization",
                        principalColumn: "OrganizationID");
                    table.ForeignKey(
                        name: "FK__PROJ_Proj__Proje__160F4887",
                        column: x => x.ProjectID,
                        principalTable: "PROJ_Project",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK__PROJ_Proj__UserI__17F790F9",
                        column: x => x.UserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "SEC_RoleClaim",
                columns: table => new
                {
                    RoleClaimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    ClaminType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClaimValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SEC_Role__BB90E97636D64882", x => x.RoleClaimID);
                    table.ForeignKey(
                        name: "FK__SEC_RoleC__Creat__7D439ABD",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__SEC_RoleC__Modif__7E37BEF6",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__SEC_RoleC__RoleI__7C4F7684",
                        column: x => x.RoleID,
                        principalTable: "SEC_Role",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "EXP_Expense",
                columns: table => new
                {
                    ExpenseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "INR"),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "submitted"),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EXP_Expe__1445CFF34223D76F", x => x.ExpenseID);
                    table.ForeignKey(
                        name: "FK__EXP_Expen__Categ__2EDAF651",
                        column: x => x.CategoryID,
                        principalTable: "EXP_Category",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK__EXP_Expen__Creat__31B762FC",
                        column: x => x.CreatedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__EXP_Expen__Modif__32AB8735",
                        column: x => x.ModifiedByUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__EXP_Expen__Organ__2BFE89A6",
                        column: x => x.OrganizationID,
                        principalTable: "ORG_Organization",
                        principalColumn: "OrganizationID");
                    table.ForeignKey(
                        name: "FK__EXP_Expen__Proje__2DE6D218",
                        column: x => x.ProjectID,
                        principalTable: "PROJ_Project",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK__EXP_Expen__UserI__2CF2ADDF",
                        column: x => x.UserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PROJ_ExpensePolicy",
                columns: table => new
                {
                    PolicyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    MaxAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    IsReceiptMandatory = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PROJ_Exp__2E13394431904DD5", x => x.PolicyID);
                    table.ForeignKey(
                        name: "FK__PROJ_Expe__Categ__2739D489",
                        column: x => x.CategoryID,
                        principalTable: "EXP_Category",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK__PROJ_Expe__Proje__2645B050",
                        column: x => x.ProjectID,
                        principalTable: "PROJ_Project",
                        principalColumn: "ProjectID");
                });

            migrationBuilder.CreateTable(
                name: "COM_ExpenseQuery",
                columns: table => new
                {
                    QueryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseID = table.Column<int>(type: "int", nullable: false),
                    SenderUserID = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETDATE()"),
                    IsRead = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__COM_Expe__5967F7FBBC815B6A", x => x.QueryID);
                    table.ForeignKey(
                        name: "FK__COM_Expen__Expen__3864608B",
                        column: x => x.ExpenseID,
                        principalTable: "EXP_Expense",
                        principalColumn: "ExpenseID");
                    table.ForeignKey(
                        name: "FK__COM_Expen__Sende__395884C4",
                        column: x => x.SenderUserID,
                        principalTable: "SEC_User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_COM_ExpenseQuery_ExpenseID",
                table: "COM_ExpenseQuery",
                column: "ExpenseID");

            migrationBuilder.CreateIndex(
                name: "IX_COM_ExpenseQuery_SenderUserID",
                table: "COM_ExpenseQuery",
                column: "SenderUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Category_CreatedByUserID",
                table: "EXP_Category",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Category_ModifiedByUserID",
                table: "EXP_Category",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Category_OrganizationID",
                table: "EXP_Category",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Category_ProjectID",
                table: "EXP_Category",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Expense_CategoryID",
                table: "EXP_Expense",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Expense_CreatedByUserID",
                table: "EXP_Expense",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Expense_ModifiedByUserID",
                table: "EXP_Expense",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Expense_OrganizationID",
                table: "EXP_Expense",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Expense_ProjectID",
                table: "EXP_Expense",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Expense_UserID",
                table: "EXP_Expense",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Report_CreatedByUserID",
                table: "EXP_Report",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Report_ManagerID",
                table: "EXP_Report",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Report_ModifiedByUserID",
                table: "EXP_Report",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Report_OrganizationID",
                table: "EXP_Report",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_EXP_Report_ProjectID",
                table: "EXP_Report",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ORG_Organization_CreatedByUserID",
                table: "ORG_Organization",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ORG_Organization_ModifiedByUserID",
                table: "ORG_Organization",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "UQ__ORG_Orga__F50959E41C5632A3",
                table: "ORG_Organization",
                column: "OrganizationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_ExpensePolicy_CategoryID",
                table: "PROJ_ExpensePolicy",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_ExpensePolicy_ProjectID",
                table: "PROJ_ExpensePolicy",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_Project_CreatedByUserID",
                table: "PROJ_Project",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_Project_ManagerID",
                table: "PROJ_Project",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_Project_ModifiedByUserID",
                table: "PROJ_Project",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_Project_OrganizationID",
                table: "PROJ_Project",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "PROJ_Project_ORG",
                table: "PROJ_Project",
                columns: new[] { "ProjectName", "OrganizationID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_ProjectMember_OrganizationID",
                table: "PROJ_ProjectMember",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_ProjectMember_ProjectID",
                table: "PROJ_ProjectMember",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJ_ProjectMember_UserID",
                table: "PROJ_ProjectMember",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_Role_CreatedByUserID",
                table: "SEC_Role",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_Role_ModifiedByUserID",
                table: "SEC_Role",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "UK_SEC_Role_Org_Name",
                table: "SEC_Role",
                columns: new[] { "OrganizationID", "RoleName" },
                unique: true,
                filter: "[OrganizationID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_RoleClaim_CreatedByUserID",
                table: "SEC_RoleClaim",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_RoleClaim_ModifiedByUserID",
                table: "SEC_RoleClaim",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_RoleClaim_RoleID",
                table: "SEC_RoleClaim",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_User_CreatedByUserID",
                table: "SEC_User",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_User_ModifiedByUserID",
                table: "SEC_User",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "UQ__SEC_User__08638DF858550AC5",
                table: "SEC_User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__SEC_User__1D6F012637269705",
                table: "SEC_User",
                column: "MobileNo",
                unique: true,
                filter: "[MobileNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_UserAuth_OrganizationID",
                table: "SEC_UserAuth",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_UserRole_CreatedByUserID",
                table: "SEC_UserRole",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_UserRole_ModifiedByUserID",
                table: "SEC_UserRole",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_UserRole_UserID",
                table: "SEC_UserRole",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COM_ExpenseQuery");

            migrationBuilder.DropTable(
                name: "EXP_Report");

            migrationBuilder.DropTable(
                name: "LOG_COM_ExpenseQuery");

            migrationBuilder.DropTable(
                name: "LOG_ErrorDBMS");

            migrationBuilder.DropTable(
                name: "LOG_EXP_Category");

            migrationBuilder.DropTable(
                name: "LOG_EXP_Expense");

            migrationBuilder.DropTable(
                name: "LOG_EXP_Report");

            migrationBuilder.DropTable(
                name: "LOG_ORG_Organization");

            migrationBuilder.DropTable(
                name: "LOG_PROJ_Project");

            migrationBuilder.DropTable(
                name: "LOG_PROJ_ProjectMember");

            migrationBuilder.DropTable(
                name: "LOG_SEC_RoleClaim");

            migrationBuilder.DropTable(
                name: "LOG_SEC_User");

            migrationBuilder.DropTable(
                name: "LOG_SEC_UserAuth");

            migrationBuilder.DropTable(
                name: "LOG_SEC_UserRole");

            migrationBuilder.DropTable(
                name: "MST_SPExecution");

            migrationBuilder.DropTable(
                name: "PROJ_ExpensePolicy");

            migrationBuilder.DropTable(
                name: "PROJ_ProjectMember");

            migrationBuilder.DropTable(
                name: "SEC_Operation");

            migrationBuilder.DropTable(
                name: "SEC_RoleClaim");

            migrationBuilder.DropTable(
                name: "SEC_UserAuth");

            migrationBuilder.DropTable(
                name: "SEC_UserRole");

            migrationBuilder.DropTable(
                name: "EXP_Expense");

            migrationBuilder.DropTable(
                name: "SEC_Role");

            migrationBuilder.DropTable(
                name: "EXP_Category");

            migrationBuilder.DropTable(
                name: "PROJ_Project");

            migrationBuilder.DropTable(
                name: "ORG_Organization");

            migrationBuilder.DropTable(
                name: "SEC_User");
        }
    }
}

using Common.PermissionModule;

namespace Reimbit.Application.Common.Module;

public class Modules
{
    public const string EmployeeManagement = "Employee Management";
    public const string Projects = "Projects";
    public const string Expenses = "Expenses";
    public const string Reports = "Reports";
    public const string Payouts = "Payouts";
}

public enum SubModules
{
    #region EnployeeManagement 101 - 200
    [ModuleMetaData(GroupName = Modules.EmployeeManagement, Name = "Employee List", Description = "", Order = 1, Icon = "", IsMenu = true)]
    EmployeeList = 101,
    [ModuleMetaData(GroupName = Modules.EmployeeManagement, Name = "Add/Edit Employee", Description = "", Order = 2, Icon = "", IsMenu = true)]
    AddEditEmployee = 102,
    [ModuleMetaData(GroupName = Modules.EmployeeManagement, Name = "Role Management", Description = "", Order = 3, Icon = "", IsMenu = true)]
    RoleManagement = 103,
    [ModuleMetaData(GroupName = Modules.EmployeeManagement, Name = "Permissions", Description = "", Order = 4, Icon = "", IsMenu = true)]
    Permissions = 104,
    #endregion

    #region Projects 201 - 300
    [ModuleMetaData(GroupName = Modules.Projects, Name = "Project Master List", Description = "", Order = 1, Icon = "", IsMenu = true)]
    ProjectList = 201,
    [ModuleMetaData(GroupName = Modules.Projects, Name = "Create/Edit Project", Description = "", Order = 2, Icon = "", IsMenu = true)]
    CreateEditProject = 202,
    [ModuleMetaData(GroupName = Modules.Projects, Name = "Policy Configuration", Description = "", Order = 3, Icon = "", IsMenu = true)]
    Policies = 203,
    [ModuleMetaData(GroupName = Modules.Projects, Name = "Category Management", Description = "", Order = 4, Icon = "", IsMenu = true)]
    CategoryManagement = 204,
    #endregion

    #region Expenses 301 - 400
    [ModuleMetaData(GroupName = Modules.Expenses, Name = "My Expense", Description = "", Order = 1, Icon = "", IsMenu = true)]
    MyExpense = 301,
    [ModuleMetaData(GroupName = Modules.Expenses, Name = "Approval Inbox", Description = "", Order = 2, Icon = "", IsMenu = true)]
    ApprovalInbox = 302,
    #endregion

    #region Reports 401 - 500
    [ModuleMetaData(GroupName = Modules.Reports, Name = "Report Generation", Description = "", Order = 1, Icon = "", IsMenu = true)]
    ReportGenertion = 401,
    [ModuleMetaData(GroupName = Modules.Reports, Name = "My Reports", Description = "", Order = 1, Icon = "", IsMenu = true)]
    MyReports = 402,
    #endregion

    #region Payouts 501 - 600
    [ModuleMetaData(GroupName = Modules.Payouts, Name = "Payout Requests", Description = "", Order = 1, Icon = "", IsMenu = true)]
    PayoutRequests = 501,
    [ModuleMetaData(GroupName = Modules.Payouts, Name = "Transaction History", Description = "", Order = 1, Icon = "", IsMenu = true)]
    TransactionHistory = 502,
    #endregion
}

namespace Reimbit.Core.Common.Permissions;

public enum Permission
{
    // Expense
    ExpenseCreate = 101,
    ExpenseViewAll = 102,
    ExpenseApprove = 103,
    ExpenseReject = 104,

    // Reports
    ReportView = 201,
    ReportGenerate = 202,
    ReportApprove = 203,

    // Finance
    FinanceView = 301,
    FinancePay = 302,

    // Admin
    UserManage = 401,
    RoleManage = 402,
    ApprovalRuleManage = 403
}

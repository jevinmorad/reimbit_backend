namespace Reimbit.Core.Common.Permissions;

public enum Permission
{
    // Expense
    Expense_Create = 101,
    Expense_ViewAll = 102,
    Expense_Approve = 103,
    Expense_Reject = 104,

    // Reports
    Report_View = 201,
    Report_Generate = 202,
    Report_Approve = 203,

    // Finance
    Finance_View = 301,
    Finance_Pay = 302,

    // Admin
    User_Manage = 401,
    Role_Manage = 402,
    ApprovalRule_Manage = 403
}

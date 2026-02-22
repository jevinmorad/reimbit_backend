namespace Reimbit.Contracts.Expenses;

public enum ExpenseStatusEnum : byte
{
    Draft = 1,
    Submitted = 2,
    UnderApproval = 3,
    Approved = 4,
    Rejected = 5,
    Paid = 6
}
namespace Reimbit.Contracts.ExpenseReports;

public enum ExpenseReportStatus : byte
{
    Open = 1,
    Submitted = 2,
    Approved = 3,
    SentToFinance = 4,
    Paid = 5,
    Closed = 6
}
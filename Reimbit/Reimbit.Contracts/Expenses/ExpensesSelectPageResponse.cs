using AegisInt.Core;

namespace Reimbit.Contracts.Expenses;

public class ExpensesSelectPageResponse
{
    public EncryptedInt ExpenseId { get; set; }
    public required string Title { get; set; }
    public EncryptedInt CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public DateTime Created { get; set; }
}
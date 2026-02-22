using AegisInt.Core;

namespace Reimbit.Contracts.Expenses;

public class ExpenseSelectPkResponse
{
    public EncryptedInt ExpenseId { get; set; }
    public EncryptedInt CategoryId { get; set; }
    public string? Title { get; set; }
    public required decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? ReceiptUrl { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public DateTime Created { get; set; }
}

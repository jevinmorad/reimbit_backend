using AegisInt.Core;

namespace Reimbit.Contracts.Expenses;

public class GetExpenseResponse
{
    public EncryptedInt ExpenseId { get; set; }
    //public EncryptedInt ProjectId { get; set; }
    public string ProjectName { get; set; }
    public EncryptedInt CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? ReceiptUrl { get; set; }
    public string? Description { get; set; }
    public byte Status { get; set; }
    public DateTime Created { get; set; }
}

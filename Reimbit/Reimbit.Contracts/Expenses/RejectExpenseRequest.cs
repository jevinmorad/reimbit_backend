using AegisInt.Core;

namespace Reimbit.Contracts.Expenses;

public class RejectExpenseRequest
{
    public required EncryptedInt ExpenseId { get; set; }
    public required string RejectionReason { get; set; }
    public required int ModifiedByUserId { get; set; }
}
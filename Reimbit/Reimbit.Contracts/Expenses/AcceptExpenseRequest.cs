using AegisInt.Core;

namespace Reimbit.Contracts.Expenses;

public class AcceptExpenseRequest
{
    public required EncryptedInt ExpenseId { get; set; }
    public required int ModifiedByUserId { get; set; }
}
using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Expenses;

public class RejectExpenseRequest
{
    public required EncryptedInt ExpenseId { get; set; }
    public required string RejectionReason { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
}
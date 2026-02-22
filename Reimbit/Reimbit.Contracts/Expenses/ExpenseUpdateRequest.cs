using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Expenses;

public class ExpenseUpdateRequest
{
    public required EncryptedInt ExpenseId { get; set; }
    public required EncryptedInt CategoryId { get; set; }
    public required string Title { get; set; }
    public required decimal Amount { get; set; }
    public string? Currency { get; set; } = "INR";
    public string? ReceiptUrl { get; set; } = null;
    public string? Description { get; set; } = null;

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
}
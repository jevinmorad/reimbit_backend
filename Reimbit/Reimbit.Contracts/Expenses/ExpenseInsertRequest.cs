using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Expenses;

public class ExpenseInsertRequest
{
    public required EncryptedInt CategoryId { get; set; }
    public required string Title { get; set; }
    public required decimal Amount { get; set; } = 0;
    public string? Currency { get; set; } = "INR";
    public string? ReceiptUrl { get; set; } = null;
    public string? Description { get; set; } = null;

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
}

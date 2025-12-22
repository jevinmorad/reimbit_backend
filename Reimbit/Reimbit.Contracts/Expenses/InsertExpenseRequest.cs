using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Expenses;

public class InsertExpenseRequest
{
    public required EncryptedInt ProjectId { get; set; }
    public required EncryptedInt CategoryId { get; set; }
    public required string Title { get; set; }
    public required decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? AttachmentUrl { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public EncryptedInt UserId { get; set; }
    [JsonIgnore]
    public string ExpenseStatus { get; set; } = "submitted";
    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
}

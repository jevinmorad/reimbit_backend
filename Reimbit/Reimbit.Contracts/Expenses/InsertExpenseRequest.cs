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
    public int OrganizationId { get; set; }
    public EncryptedInt UserId { get; set; }
    public string ExpenseStatus { get; set; } = "submitted";
    public int CreatedByUserId { get; set; }
    public int ModifiedByUserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}

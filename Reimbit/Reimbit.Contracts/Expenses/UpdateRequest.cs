using AegisInt.Core;

namespace Reimbit.Contracts.Expenses;

public class UpdateRequest
{
    public required EncryptedInt ExpenseId { get; set; }
    public required EncryptedInt ProjectId { get; set; }
    public required EncryptedInt CategoryId { get; set; }
    public required string Title { get; set; }
    public required decimal Amount { get; set; }
    public string ExpenseStatus { get; set; }
    public string? Currency { get; set; }
    public string? AttachmentUrl { get; set; }
    public string? Description { get; set; }
    public int OrganizationId { get; set; }
    public int ModifiedByUserId { get; set; }
    public DateTime Modified { get; set; }
}
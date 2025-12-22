using AegisInt.Core;

namespace Reimbit.Contracts.Expenses;

public class ViewExpenseResponse
{
    public EncryptedInt ExpenseId { get; set; }
    public required string Title { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public string? Description { get; set; }
    public string? AttachmentUrl { get; set; }
    public required string ExpenseStatus { get; set; }
    public string? RejectionReason { get; set; }
    public required EncryptedInt ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public required EncryptedInt CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public required string UserDisplayName { get; set; }
    public required string CreatedByUserDisplayName { get; set; }
    public required string ModifiedByUserDisplayName { get; set; }
    public DateTime Created { get; set; }
    public required DateTime Modified { get; set; }
}
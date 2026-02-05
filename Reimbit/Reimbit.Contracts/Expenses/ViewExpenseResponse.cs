namespace Reimbit.Contracts.Expenses;

public class ViewExpenseResponse
{
    public required string Title { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public string? Description { get; set; }
    public string? AttachmentUrl { get; set; }
    public required string ExpenseStatus { get; set; }
    public string? RejectionReason { get; set; }
    public required string CategoryName { get; set; }
    public required string UserDisplayName { get; set; }
    public required string CreatedByUserDisplayName { get; set; }
    public string? ModifiedByUserDisplayName { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}
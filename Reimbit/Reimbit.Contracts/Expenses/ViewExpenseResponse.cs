using AegisInt.Core;

namespace Reimbit.Contracts.Expenses;

public class ViewExpenseResponse
{
    public EncryptedInt ExpenseId { get; set; }
    public string Title { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? Description { get; set; }
    public string? AttachmentUrl { get; set; }
    public string ExpenseStatus { get; set; } = null!;
    public string? RejectionReason { get; set; }
    
    public EncryptedInt ProjectId { get; set; }
    public string ProjectName { get; set; } = null!;
    
    public EncryptedInt CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    
    public string UserDisplayName { get; set; } = null!;
    public string CreatedByUserDisplayName { get; set; } = null!;
    public string ModifiedByUserDisplayName { get; set; } = null!;
    
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}
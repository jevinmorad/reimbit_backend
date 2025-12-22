using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Expenses;

public class GetExpenseResponse
{
    public EncryptedInt ExpenseId { get; set; }
    public EncryptedInt ProjectId { get; set; }
    public string ProjectName { get; set; }
    public EncryptedInt CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? AttachmentUrl { get; set; }
    public string? Description { get; set; }
    public string ExpenseStatus { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime Created { get; set; }
}

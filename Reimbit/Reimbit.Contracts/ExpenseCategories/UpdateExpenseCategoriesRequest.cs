using AegisInt.Core;

namespace Reimbit.Contracts.ExpenseCategories;

public class UpdateExpenseCategoriesRequest
{
    public required EncryptedInt CategoryId { get; set; }
    public required EncryptedInt ProjectId { get; set; }
    public required string CategoryName { get; set; }
    public string? Description { get; set; }
    public int OrganizationId { get; set; }
    public int ModifiedByUserId { get; set; }
    public DateTime Modified { get; set; }
}
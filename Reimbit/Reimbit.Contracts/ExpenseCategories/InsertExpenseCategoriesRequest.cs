using AegisInt.Core;

namespace Reimbit.Contracts.ExpenseCategories;

public class InsertExpenseCategoriesRequest

{
    public required EncryptedInt ProjectId { get; set; }
    public required string CategoryName { get; set; }
    public string? Description { get; set; }
    public int OrganizationId { get; set; }
    public int CreatedByUserId { get; set; }
    public int? ModifiedByUserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}
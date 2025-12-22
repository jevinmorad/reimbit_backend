using AegisInt.Core;

namespace Reimbit.Contracts.ExpenseCategories;

public class GetExpenseCategoriesResponse
{
    public EncryptedInt CategoryId { get; set; }
    public EncryptedInt ProjectId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
}
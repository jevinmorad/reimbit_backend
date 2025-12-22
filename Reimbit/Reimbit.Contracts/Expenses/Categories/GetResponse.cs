using AegisInt.Core;

namespace Reimbit.Contracts.Expenses.Categories;

public class GetResponse
{
    public EncryptedInt CategoryId { get; set; }
    public EncryptedInt ProjectId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
}
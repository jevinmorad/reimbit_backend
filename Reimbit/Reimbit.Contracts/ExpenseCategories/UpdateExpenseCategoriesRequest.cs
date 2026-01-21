using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.ExpenseCategories;

public class UpdateExpenseCategoriesRequest
{
    public required EncryptedInt CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public string? Description { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
}
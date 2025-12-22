using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.ExpenseCategories;

public class InsertExpenseCategoriesRequest

{
    public required EncryptedInt ProjectId { get; set; }
    public required string CategoryName { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public int? ModifiedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
}
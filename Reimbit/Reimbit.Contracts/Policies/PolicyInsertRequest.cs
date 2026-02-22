using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Policies;

public class PolicyInsertRequest
{
    public required EncryptedInt CategoryId { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool IsReceiptMandatory { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
}
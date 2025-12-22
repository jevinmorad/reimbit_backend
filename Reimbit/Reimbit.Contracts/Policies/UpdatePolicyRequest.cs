using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Policies;

public class UpdatePolicyRequest
{
    public required EncryptedInt PolicyId { get; set; }
    public required EncryptedInt ProjectId { get; set; }
    public EncryptedInt? CategoryId { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool IsReceiptMandatory { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
}
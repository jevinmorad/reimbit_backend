using AegisInt.Core;

namespace Reimbit.Contracts.Policies;

public class InsertPolicyRequest
{
    public required EncryptedInt ProjectId { get; set; }
    public EncryptedInt? CategoryId { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool IsReceiptMandatory { get; set; }
    public string? Description { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime Created { get; set; }
}
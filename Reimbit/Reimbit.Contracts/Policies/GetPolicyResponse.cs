using AegisInt.Core;

namespace Reimbit.Contracts.Policies;

public class GetPolicyResponse
{
    public EncryptedInt PolicyId { get; set; }
    public EncryptedInt ProjectId { get; set; }
    public EncryptedInt? CategoryId { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool IsReceiptMandatory { get; set; }
    public string? Description { get; set; }
}

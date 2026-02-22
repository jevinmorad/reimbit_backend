using AegisInt.Core;

namespace Reimbit.Contracts.Policies;

public class PolicySelectPKResponse
{
    public EncryptedInt PolicyId { get; set; }
    public EncryptedInt? CategoryId { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool IsReceiptMandatory { get; set; }
    public string? Description { get; set; }
}

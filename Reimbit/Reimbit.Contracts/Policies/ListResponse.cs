using AegisInt.Core;

namespace Reimbit.Contracts.Policies;

public class ListResponse
{
    public EncryptedInt PolicyId { get; set; }
    public EncryptedInt ProjectId { get; set; }
    public string ProjectName { get; set; }
    public EncryptedInt? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool IsReceiptMandatory { get; set; }
    public string? Description { get; set; }
    public DateTime? Created { get; set; }
}

using AegisInt.Core;

namespace Reimbit.Contracts.Security.Operation;

public class UpdateRequest
{
    public EncryptedInt OperationID { get; set; }
    public string OperationName { get; set; }
    public bool? IsDeveloperOperation { get; set; }
    public bool? IsActive { get; set; }
    public decimal? Sequence { get; set; }
    public string Description { get; set; }
    public int ModifiedByUserID { get; set; }
}
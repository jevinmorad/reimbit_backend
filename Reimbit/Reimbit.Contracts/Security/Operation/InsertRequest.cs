
using AegisInt.Core;

namespace GNLib.Contracts.Security.Operation;

public class InsertRequest
{
    public EncryptedInt? OperationID { get; set; }
    public EncryptedInt ModuleID { get; set; }
    public EncryptedInt SubModuleID { get; set; }
    public int OperationNo { get; set; }
    public string OperationName { get; set; }
    public bool? IsDeveloperOperation { get; set; }
    public bool? IsActive { get; set; }
    public decimal? Sequence { get; set; }
    public string Description { get; set; }
    public int CreatedByUserID { get; set; }
    public int ModifiedByUserID { get; set; }
}
using AegisInt.Core;
using Common.Data.Models;

namespace Reimbit.Contracts.Security.Operation;

public class ListResponse : ListItemBase
{
    public EncryptedInt OperationID { get; set; }
    public int ModuleNo { get; set; }
    public int SubModuleNo { get; set; }
    public string ModuleName { get; set; }
    public string SubModuleName { get; set; }
    public int OperationNo { get; set; }
    public string OperationName { get; set; }
    public bool? IsDeveloperOperation { get; set; }
    public bool? IsActive { get; set; }
    public decimal? Sequence { get; set; }
    public string Description { get; set; }
}
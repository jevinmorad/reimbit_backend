namespace Reimbit.Contracts.Security.Operaton;

public class GetResponse
{
    public int OperationID { get; set; }
    public int ModuleNo { get; set; }
    public int SubModuleNo { get; set; }
    public int ModuleID { get; set; }
    public int SubModuleID { get; set; }
    public int OperationNo { get; set; }
    public string OperationName { get; set; }
    public bool? IsActive { get; set; }
    public decimal? Sequence { get; set; }
    public string Description { get; set; }
}
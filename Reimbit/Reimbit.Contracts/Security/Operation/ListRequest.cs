using Common.NetStandard.Web.Models;

namespace GNLib.Contracts.Security.Operation;

public class ListRequest : PagedRequest
{
    public int? OperationNo { get; set; }    
    public string OperationName { get; set; }    
}
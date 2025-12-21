using Common.Web.Models;

namespace Reimbit.Contracts.Security.Operation;

public class ListRequest : PagedRequest
{
    public int? OperationNo { get; set; }    
    public string OperationName { get; set; }    
}
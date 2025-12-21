using Common.Web.Models;

namespace Reimbit.Contracts.Security.UserRole;

public class ListRequest : PagedRequest
{
    public int? UserID { get; set; }
    public int? RoleID { get; set; }

}

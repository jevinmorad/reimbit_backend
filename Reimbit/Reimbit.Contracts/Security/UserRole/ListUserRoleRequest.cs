using Reimbit.Core.Common.NetStandard.Web.Models;

namespace GNLib.Contracts.Security;

public class ListUserRoleRequest : PagedRequest
{
    public int? UserID { get; set; }
    public int? RoleID { get; set; }

}

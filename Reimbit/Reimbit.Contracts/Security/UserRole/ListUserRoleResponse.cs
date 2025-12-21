using Common.Data.Models;

namespace Reimbit.Contracts.Security.UserRole;

public class ListResponse : ListItemBase
{
	public string UserName { get; set; }
	public string UserRoleName { get; set; }
	public string Permissions { get; set; }
	public int? UserRoleAccessID { get; set; }
}

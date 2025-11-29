using Reimbit.Core.Common.NetStandard.Data.Models;

namespace GNLib.Contracts.Security;

public class ListUserRoleResponse : ListItemBase
{
	public string UserName { get; set; }
	public string UserRoleName { get; set; }
	public string Permissions { get; set; }
	public int? UserRoleAccessID { get; set; }
}

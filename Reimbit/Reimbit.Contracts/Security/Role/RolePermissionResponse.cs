using Common.Data.Models;

namespace Reimbit.Contracts.Security.Role;

public class RolePermissionResponse
{
	public List<RolePermissionResponseItem> Permissions { get; set; }
}

public class RolePermissionResponseItem
{
	public int ModuleId { get; set; }
	public string ModuleName { get; set; }
	public List<OptionsResponse<int>> Permissions { get; set; }
}

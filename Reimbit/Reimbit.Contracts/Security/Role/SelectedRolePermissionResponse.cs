namespace Reimbit.Contracts.Security.Role;

public class SelectedRolePermissionResponse
{
	public List<SelectedRolePermissionResponseItem> Permissions { get; set; }
}

public class SelectedRolePermissionResponseItem
{
	public int ModuleId { get; set; }
	public string ModuleName { get; set; }
	public List<int> Permissions { get; set; }
}

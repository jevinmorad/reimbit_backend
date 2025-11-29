namespace Reimbit.Contracts.Security.Role;

public class RolePermissionRequest
{
    public int RoleID { get; set; }
    public List<RolePermissionItemRequest> Permission { get; set; }
}

public class RolePermissionItemRequest
{
    public int ModuleID { get; set; }
    public string[] Permissions { get; set; }
}
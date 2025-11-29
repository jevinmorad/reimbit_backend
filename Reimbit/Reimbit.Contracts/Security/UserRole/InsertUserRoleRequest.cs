namespace Reimbit.Contracts.Security.UserRole;

public class InsertUserRoleRequest
{
	public int UserID { get; set; }
	public int RoleID { get; set; }
	public int OrganizationID { get; set; }
	public string UniqueKey { get; set; }
}

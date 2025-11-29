using Common.NetCore.RequestEncryption;

namespace GNLib.Contracts.Security;

public class UpdateUserRoleRequest
{
	public int UserID { get; set; }
	public int RoleID { get; set; }
	public int OrganizationID { get; set; }
	public string UniqueKey { get; set; }
}

using AegisInt.Core;

namespace GNLib.Contracts.Security;

public class UpdateUserRoleRequest
{
	public required EncryptedInt UserID { get; set; }
	public required EncryptedInt RoleID { get; set; }
	public required int OrganizationID { get; set; }
}

using AegisInt.Core;

namespace Reimbit.Contracts.Security.UserRole;

public class UpdateRequest
{
	public required EncryptedInt UserID { get; set; }
	public required EncryptedInt RoleID { get; set; }
	public required int OrganizationID { get; set; }
}
